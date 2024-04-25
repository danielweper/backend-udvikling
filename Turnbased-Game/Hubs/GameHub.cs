using System.Drawing;
using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Server;
using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.Server;
using static Turnbased_Game.Models.Color;
using Color = Turnbased_Game.Models.Color;
using IClient = Turnbased_Game.Models.Server.IClient;

namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IClient>
{
    private const GameType DefaultGameType = GameType.RoundRobin;

    private readonly Server _server = new();
    private readonly Random _random = new();


    public override async Task OnConnectedAsync()
    {
        await SendMessagePacket("You have successfully joined the lobby", MessageType.Acknowledged, Clients.Caller);
    }


    // public async Task CreatePlayerProfile(string name, Color color = Red)
    // {
    //     
    //     await SendMessagePacket()
    //     
    // }

    public async Task CreateLobby(int maxPlayerCount, LobbyVisibility lobbyVisibility, PlayerProfile playerProfile)
    {
        await SendMessagePacket("Received CreateLobby request", MessageType.Acknowledged,
            Clients.Caller); // Acknowledged

        // Create host
        Player host = new Player(GenerateParticipantId(null), playerProfile);

        // Create lobby
        byte lobbyId = GenerateLobbyId();
        Lobby lobby = new Lobby(lobbyId, host, maxPlayerCount, lobbyVisibility);
        _server.AddLobby(lobby);

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        await SendMessagePacket("Lobby created", MessageType.Accepted, Clients.Caller);

        LobbyInfo lobbyInfo = lobby.GetInfo();
        await Clients.Caller.PlayerJoiningLobby(new LobbyInfoPacket(lobbyInfo));
    }

    public async Task JoinLobby(byte lobbyId, PlayerProfile playerProfile)
    {
        var lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "No lobby found with corresponding id",
                type: MessageType.Denied);
            return;
        }

        Player player = new Player(GenerateParticipantId(lobby), playerProfile);

        lobby.AddPlayer(player);
        await SendMessagePacket(message: $"You have successfully joined lobby: {lobbyId}", type: MessageType.Accepted,
            caller: Clients.Caller);
        LobbyInfoPacket packet = new LobbyInfoPacket(lobby.GetInfo());
        await Clients.Caller.PlayerJoiningLobby(packet);

        // TODO: make actual player profile
        await Clients.Group($"{lobbyId}")
            .PlayerHasJoined(new PlayerJoinedLobbyPacket(playerId: player.ParticipantId, playerProfile));
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
    }

    public async Task KickPlayerFromLobby(byte playerIdToKick, string reason, byte lobbyId)
    {
        var lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "No lobby found with corresponding id",
                type: MessageType.Denied);
            return;
        }

        var player = lobby.GetPlayer(playerIdToKick);

        if (player != null)
        {
            //Remove the player from the lobby
            lobby.RemovePlayer(player);

            //Update the players ID
            lobby.UpdatePlayerId();

            await SendMessagePacket(
                message: $"You have successfully kicked player {player.ParticipantId} from this lobby: {lobbyId}",
                type: MessageType.Accepted, caller: Clients.Caller);
            await Clients.Group($"{lobbyId}")
                .PlayerKicked(new KickPlayerPacket(playerId: player.ParticipantId, reason: reason));
        }
        else
        {
            await SendMessagePacket(caller: Clients.Caller, message: $"The player is not in this lobby: {lobbyId}",
                type: MessageType.Denied);
        }
    }

    public async Task LeaveLobby(byte lobbyId, byte playerId)
    {
        var lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "This lobby does not exist",
                type: MessageType.Denied);
            return;
        }

        var player = lobby.GetPlayer(playerId);

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
                lobby.UpdatePlayerId();

                //Send packet that a player left
                await Clients.Group($"{lobbyId}").PlayerHasLeft(new PlayerLeftLobbyPacket(player.ParticipantId));
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

        var lobby = _server.GetLobby(lobbyId);


        if (lobby != null)
        {
            Game game = lobby.CreateNewGame(gameType);
            await Clients.Group($"{lobbyId}").GameCreated(new GameInfoPacket(game.GetInfo()));
            await SendMessagePacket("Game created", MessageType.Accepted, Clients.Caller);
        }
        else
        {
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
        }
    }

    public async Task StartGame(byte lobbyId)
    {
        // Acknowledged
        await SendMessagePacket("Received Start Game request", MessageType.Acknowledged, Clients.Caller);

        var lobby = _server.GetLobby(lobbyId);

        //Check of a lobby has a game
        if (lobby?.GetGame() != null)
        {
            bool playersReady = lobby.Players.All(p => p.ReadyStatus);
            if (playersReady)
            {
                lobby.StartGame();

                await Clients.Group($"{lobbyId}").StartGame(new GameStartingPacket(DateTime.Now));
                await SendMessagePacket("Game Started", MessageType.Accepted, Clients.Caller);
            }
            else
            {
                await SendMessagePacket("All Players are not ready", MessageType.Denied, Clients.Caller);
            }
        }
        else
        {
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
        }
    }

    public async Task ToggleIsPlayerReady(byte lobbyId, byte playerId, bool ready)
    {
        // Acknowledged
        await SendMessagePacket("Received player is Ready request", MessageType.Acknowledged, Clients.Caller);

        var lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "This lobby does not exist",
                type: MessageType.Denied);
            return;
        }

        //Get player
        var player = lobby.GetPlayer(playerId);

        if (player != null)
        {
            player.ReadyStatus = ready;
            //Send package
            await Clients.Group($"{lobbyId}").ToggleReadyToStart(new PlayerReadyStatusPacket(ready, playerId));

            if (!ready)
            {
                await SendMessagePacket("You are ready to play", MessageType.Accepted, Clients.Caller);
            }
            else
            {
                await SendMessagePacket("You are not ready to play", MessageType.Accepted, Clients.Caller);
            }
        }
        else
        {
            await SendMessagePacket(caller: Clients.Caller, message: "You are not in this lobby",
                type: MessageType.Denied);
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
            await SendMessagePacket("You have successfully changed game settings", MessageType.Accepted,
                Clients.Caller);
        }
        else
        {
            await SendMessagePacket("The game doesn't exist", MessageType.Denied, Clients.Caller);
        }
    }

    public async Task ChangePlayerProfile(byte lobbyId, byte playerId, PlayerProfile newPlayerProfile)
    {
        await SendMessagePacket($"Request to change player profile from playerId: {playerId}, received",
            MessageType.Acknowledged,
            Clients.Caller);
        var lobby = _server.GetLobby(lobbyId);


        if (lobby == null)
        {
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
            return;
        }

        var player = lobby.GetPlayer(playerId);

        if (player == null)
        {
            await SendMessagePacket("The player doesn't exist in the lobby", MessageType.Denied, Clients.Caller);
            return;
        }

        player.Profile = newPlayerProfile;

        await Clients.OthersInGroup($"{lobbyId}")
            .PlayerProfileUpdated(new PlayerProfileChangedPacket(playerId, newPlayerProfile));
        await SendMessagePacket(
            $"You have successfully changed your player profile. New player profile: {newPlayerProfile}",
            MessageType.Accepted,
            Clients.Caller);
    }

    public async Task RoleChange(byte playerId, byte lobbyId, PlayerRole newPlayerRole)
    {
        await SendMessagePacket($"Request to change role: {playerId}, received", MessageType.Acknowledged,
            Clients.Caller);
        var lobby = _server.GetLobby(lobbyId);


        if (lobby == null)
        {
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
            return;
        }

        var player = lobby.GetPlayer(playerId);

        if (player == null)
        {
            await SendMessagePacket("The player doesn't exist in the lobby", MessageType.Denied, Clients.Caller);
            return;
        }


        await SendMessagePacket($"You have successfully changed role. New role: {newPlayerRole}", MessageType.Accepted,
            Clients.Caller);
        await Clients.OthersInGroup($"{lobbyId}")
            .PlayerRoleChanged(new PlayerRoleChangedPacket(playerId, newPlayerRole));
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
        HashSet<byte> usedIds = lobby?.Players.Select(p => p.ParticipantId).ToHashSet() ?? new HashSet<byte>();

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
                AcknowledgedPacket acknowledgedPacket = new(message, DateTime.Now);
                await caller.ReceiveAcknowledgePacket(acknowledgedPacket);
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
                AcknowledgedPacket acknowledgedPacket = new(message, DateTime.Now);
                await Clients.Group($"{lobbyId}").ReceiveAcknowledgePacket(acknowledgedPacket);
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