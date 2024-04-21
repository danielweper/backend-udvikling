using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Server;
using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.Server;
using IClient = Turnbased_Game.Models.Server.IClient;

namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IClient>
{
    private const GameType DefaultGameType = GameType.RoundRobin;
    
    private readonly Server _server = new();
    private readonly Random _random = new();

    public async Task CreateLobby(int maxPlayerCount)
    {
        await SendMessagePacket("Received CreateLobby request", MessageType.Acknowledged,
            Clients.Caller); // Acknowledged

        // Create host
        Player host = new Player(GenerateParticipantId(null));

        // Create lobby
        byte lobbyId = GenerateLobbyId();
        Lobby lobby = new Lobby(lobbyId, host);
        _server.AddLobby(lobby);

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        CreateLobbyPacket packet = new CreateLobbyPacket(lobbyId);
        await SendMessagePacket("Lobby created", MessageType.Accepted, Clients.Caller);

        LobbyInfo lobbyInfo = lobby.GetInfo();
        await Clients.Caller.PlayerJoiningLobby(new LobbyInfoPacket(lobbyInfo));
    }

    public async Task JoinLobby(byte lobbyId)
    {
        Lobby? lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "No lobby found with corresponding id",
                type: MessageType.Denied);
            return;
        }

        Player player = new Player(GenerateParticipantId(lobby));

        lobby.AddPlayer(player);
        await SendMessagePacket(message: $"You have successfully joined lobby: {lobbyId}", type: MessageType.Accepted,
            caller: Clients.Caller);
        LobbyInfoPacket packet = new LobbyInfoPacket(lobby.GetInfo());
        await Clients.Caller.PlayerJoiningLobby(packet);

        // TODO: make actual player profile
        await Clients.Group($"{lobbyId}")
            .PlayerHasJoined(new PlayerJoinedLobbyPacket(playerId: player.id, new PlayerProfile()));
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
    }

    public async Task KickPlayerFromLobby(byte playerId, string reason, byte lobbyId)
    {
        Lobby? lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "No lobby found with corresponding id",
                type: MessageType.Denied);
            return;
        }

        Player? player = lobby.Players.FirstOrDefault(p => p.id == playerId);

        if (player != null)
        {
            //Remove the player from the lobby
            lobby.RemovePlayer(player);

            await SendMessagePacket(
                message: $"You have successfully kicked player {player.id} from this lobby: {lobbyId}",
                type: MessageType.Accepted, caller: Clients.Caller);
            await Clients.Group($"{lobbyId}")
                .KickPlayerRequest(new KickPlayerPacket(playerId: player.id, reason: reason));
        }
        else
        {
            await SendMessagePacket(caller: Clients.Caller, message: $"The player is not in this lobby: {lobbyId}",
                type: MessageType.Denied);
        }
    }

    public async Task LeaveLobby(byte lobbyId)
    {
        Lobby? lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "This lobby does not exist",
                type: MessageType.Denied);
            return;
        }

        Player? player = lobby.Players.FirstOrDefault(p => p.id.ToString() == Context.ConnectionId);

        if (player != null)
        {
            //Remove Player
            lobby.RemovePlayer(player);

            //Check if player is the last player left in lobby
            if (lobby.PlayerCount == 0)
            {
                _server.RemoveLobby(lobby);
                await SendMessagePacket(caller: Clients.Caller, message: "The lobby has been deleted",
                    type: MessageType.Accepted);
            }
            else
            {
                //lobby.UpdatePlayerId();
                    
                //Send packet that a player left
                await Clients.Group($"{lobbyId}").DisconnectLobby(new LeaveLobbyPacket(player.id));
            }

            //Send packet to the player, that they have disconnect the lobby
            await SendMessagePacket(message: $"You have successfully disconnected from the lobby: {lobbyId}",
                type: MessageType.Accepted,
                caller: Clients.Caller);
        }
        else
        {
            await SendMessagePacket(caller: Clients.Caller, message: "You are not in this lobby",
                type: MessageType.Denied);
        }
    }

    public async Task ViewAvailableLobbies()
    {
        await SendMessagePacket("Received view AvailableLobbies", MessageType.Acknowledged,
            Clients.Caller); // Acknowledged

        //Get all the Lobbies from server
        List<LobbyInfo> lobbiesInfo = _server.GetAvailableLobbies();

        AvailableLobbiesPacket availableLobbiesPacketPacket = new AvailableLobbiesPacket(lobbiesInfo);

        //Send the packet to the client
        await Clients.Caller.ListAvailableLobbiesRequest(availableLobbiesPacketPacket);
    }

    public async Task CreateGame(byte lobbyId, GameType gameType = DefaultGameType)
    {
        // Acknowledged
        await SendMessagePacket("Received CreateGame request", MessageType.Acknowledged, Clients.Caller);

        Lobby? lobby = _server.GetLobby(lobbyId);

        if (lobby != null)
        {
            lobby.CreateNewGame(gameType);

            await Clients.Group("lobbyId").CreateGame();
            await SendMessagePacket("Game created", MessageType.Accepted, Clients.Caller);
        }
        else
        {
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
        }
    }
    
    public async Task ChangeGameSettings(Lobby lobby, GameSettings newGameSettings)
    {
        // Acknowledged
        await SendMessagePacket("Received ChangeGameSettings request", MessageType.Acknowledged, Clients.Caller);

        if (lobby.GetGame() != null)
        {
            //Set game settings to the new settings
            lobby.GetGame()!.settings.settings = newGameSettings.settings;
            
            //send packet
            await Clients.Group("lobbyId").ChangeGameSettings(new GameSettingsChangedPacket(newGameSettings.settings));
            await SendMessagePacket("You have successfully changed game settings", MessageType.Accepted, Clients.Caller);
        }
        else
        { 
            await SendMessagePacket("The game doesn't exist", MessageType.Denied, Clients.Caller); 
        }
    }
    

    private byte GenerateLobbyId()
    {
        byte lobbyId;
        do
        {
            lobbyId = (byte)_random.Next(1, 256);
        } while (!_server.LobbyIdIsFree(lobbyId));

        return lobbyId;
    }

    private byte GenerateParticipantId(Lobby? lobby)
    {
        HashSet<byte> usedIds = lobby?.Players.Select(p => p.id).ToHashSet() ?? new HashSet<byte>();

        byte participantId;
        do
        {
            participantId = (byte)_random.Next(1, 256);
        } while (!usedIds.Contains(participantId));

        return participantId;
    }
    private async Task SendMessagePacket(string message, MessageType type, IClient caller)
    {
        switch (type)
        {
            case MessageType.Acknowledged:
            {
                AcknowledgedPacket ackPack = new(message, DateTime.Now);
                await caller.ReceiveAcknowledgePacket(ackPack);
                break;
            }
            case MessageType.Accepted:
            {
                AcceptedPacket accPack = new(message, DateTime.Now);
                await caller.ReceiveAcceptedPacket(accPack);
                break;
            }
            case MessageType.Denied:
            {
                DeniedPacket deniedPacket = new(message, DateTime.Now);
                await caller.ReceiveDeniedPacket(deniedPacket);
                break;
            }
            case MessageType.Invalid:
            {
                InvalidPacket invalidPacket = new(message, DateTime.Now);
                await caller.ReceiveInvalidPacket(invalidPacket);
                break;
            }
        }
    }

    private async Task SendMessageToGroup(string message, MessageType type, int lobbyId)
    {
        switch (type)
        {
            case MessageType.Acknowledged:
            {
                AcknowledgedPacket ackPack = new(message, DateTime.Now);
                await Clients.Group($"{lobbyId}").ReceiveAcknowledgePacket(ackPack);
                break;
            }
            case MessageType.Accepted:
            {
                AcceptedPacket accPack = new(message, DateTime.Now);
                await Clients.Group($"{lobbyId}").ReceiveAcceptedPacket(accPack);
                break;
            }
            case MessageType.Denied:
            {
                DeniedPacket deniedPacket = new(message, DateTime.Now);
                await Clients.Group($"{lobbyId}").ReceiveDeniedPacket(deniedPacket);
                break;
            }
            case MessageType.Invalid:
            {
                InvalidPacket invalidPacket = new(message, DateTime.Now);
                await Clients.Group($"{lobbyId}").ReceiveInvalidPacket(invalidPacket);
                break;
            }

        }
    }

    enum MessageType
    {
        Accepted,
        Denied,
        Acknowledged,
        Invalid
    }
}