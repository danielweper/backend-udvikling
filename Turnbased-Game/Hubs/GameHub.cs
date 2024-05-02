using Microsoft.AspNetCore.SignalR;
using Core.Packets.Client;
using Core.Packets.Server;
using Core.Packets.Shared;
using ServerLogic;
using ServerLogic.Model.Fighting;
using Core.Model;

namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IHubClient>
{
    private const GameType DefaultGameType = GameType.RoundRobin;
    private const string DefaultName = "Joe";

    private readonly Server _server = new();
    private readonly Random _random = new();


    public override async Task OnConnectedAsync()
    {
        await SendMessagePacket("You have successfully joined the lobby", MessageType.Acknowledged, Clients.Caller);
    }


    public async Task CreatePlayerProfile(string name, Color color = Color.Red)
    {
        PlayerProfile playerProfile = new(color, name, Context.ConnectionId);
        await SendMessagePacket(
            $"You have successfully created a playerProfile with Color: {color.ToString()}, Name: {name}",
            MessageType.Accepted, Clients.Caller);
        // await Clients.Caller.PlayerProfileCreated(new PlayerProfileCreatedPacket(playerProfile));
    }

    public PlayerProfile DefaultPlayerProfile(string connectionId)
    {
        return new PlayerProfile(Color.Red, DefaultName, connectionId);
    }

    public async Task CreateLobby(int maxPlayerCount, LobbyVisibility lobbyVisibility,
        PlayerProfile? playerProfile = null)
    {
        await SendMessagePacket("Received CreateLobby request", MessageType.Acknowledged,
            Clients.Caller); // Acknowledged

        playerProfile ??= DefaultPlayerProfile(Context.ConnectionId);

        // Create host
        Player host = new Player(playerProfile.Name, GenerateParticipantId(null), playerProfile);

        // Create lobby
        byte lobbyId = GenerateLobbyId();
        Lobby lobby = new Lobby(lobbyId, host, maxPlayerCount, lobbyVisibility);
        _server.AddLobby(lobby);

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        await SendMessagePacket("Lobby created", MessageType.Accepted, Clients.Caller);

        LobbyInfo lobbyInfo = lobby.GetInfo();
        // await Clients.Caller.PlayerJoiningLobby(new LobbyInfoPacket(lobbyInfo));
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

        HashSet<string> playerNames = lobby.Players.Select(pl => pl.DisplayName).ToHashSet();

        string displayName = GetDisplayName(playerProfile.Name, playerNames);

        Player player = new Player(displayName, GenerateParticipantId(lobby), playerProfile);

        lobby.AddPlayer(player);
        await SendMessagePacket(message: $"You have successfully joined lobby: {lobbyId}", type: MessageType.Accepted,
            caller: Clients.Caller);
        // LobbyInfoPacket packet = new LobbyInfoPacket(lobby.GetInfo());
        // await Clients.Caller.PlayerJoiningLobby(packet);

        // TODO: make actual player profile
        // await Clients.Group($"{lobbyId}")
        //     .PlayerHasJoined(new PlayerJoinedLobbyPacket(playerId: player.ParticipantId, displayName, playerProfile));
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
    }

    private string GetDisplayName(string originalName, HashSet<string> playerNames)
    {
        string testName = originalName;
        int suffix = 1;

        while (playerNames.Contains(testName))
        {
            testName = $"{originalName} {suffix}";
            suffix++;
        }

        return testName;
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

        Player? player = lobby.Players.FirstOrDefault((Player p) => p.ParticipantId == playerIdToKick);

        if (player != null)
        {
            //Remove the player from the lobby
            lobby.RemovePlayer(player);

            await SendMessagePacket(
                message: $"You have successfully kicked player {player.ParticipantId} from this lobby: {lobbyId}",
                type: MessageType.Accepted, caller: Clients.Caller);
            // await Clients.Group($"{lobbyId}")
            //     .PlayerKicked(new KickPlayerPacket(playerId: player.ParticipantId, reason: reason));
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

        Player? player = lobby.Players.FirstOrDefault((Player p) => p.ParticipantId == playerId);

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
                // Removes player from group
                await Groups.RemoveFromGroupAsync(player.Profile.ConnectionId, $"{lobbyId}");

                //Send packet that a player left
                // await Clients.Group($"{lobbyId}").PlayerHasLeft(new PlayerLeftLobbyPacket(player.ParticipantId));
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

        // AvailableLobbiesPacket availableLobbiesPacketPacket = new AvailableLobbiesPacket(lobbiesInfo);

        //Send the packet to the client
        // await Clients.Caller.ListAvailableLobbiesRequest(availableLobbiesPacketPacket);
    }

    public async Task CreateGame(byte lobbyId, GameType gameType = DefaultGameType)
    {
        // Acknowledged
        await SendMessagePacket("Received CreateGame request", MessageType.Acknowledged, Clients.Caller);

        var lobby = _server.GetLobby(lobbyId);


        if (lobby != null)
        {
            lobby.CreateNewGame(gameType);
            Game game = lobby?.Game;
            // await Clients.Group($"{lobbyId}").GameCreated(new GameInfoPacket(game.GetInfo()));
            await SendMessagePacket("Game created", MessageType.Accepted, Clients.Caller);
        }
        else
        {
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
        }
    }

    //Start the Game(battle/battles)
    public async Task StartGame(byte lobbyId)
    {
        // Acknowledged
        await SendMessagePacket("Received Start Game request", MessageType.Acknowledged, Clients.Caller);

        var lobby = _server.GetLobby(lobbyId);

        var allPlayersReady = lobby != null && lobby.Players.All(p => p.ReadyStatus);
        if (allPlayersReady)
        {
            //Get all Fighters 
            var fighters = lobby?.Players.Where(p => p.Role == PlayerRole.Fighter).ToList();

            //Check of a lobby has a game
            var game = lobby?.Game;
            if (fighters != null && game != null)
            {
                //Check if there is even number of Fighters
                if (fighters.Count % 2 != 0)
                {
                    await SendMessagePacket("Game cannot start, need even number of Fighters", MessageType.Denied,
                        Clients.Caller);
                    return;
                }

                for (var i = 0; i < fighters.Count; i += 2)
                {
                    //Create battle
                    var battle = new Battle(GenerateBattleId(game), fighters[i], fighters[i + 1]);
                    // var battle = new Battle(GenerateBattleId(game));
                    //game.InitializeBattles(battle);

                    // //Add Two players to a battle
                    // battle.AddPlayer(fighters[i]);
                    // battle.AddPlayer(fighters[i + 1]);

                    //Add battle to game
                    game.Battles.Add(battle);
                }

                //Send packet to client
                // await Clients.Group($"{lobbyId}").StartGame(new GameStartingPacket(DateTime.Now));
                await SendMessagePacket("Game Started", MessageType.Accepted, Clients.Caller);
            }
            else
            {
                await SendMessagePacket("The game or fighters do not exist", MessageType.Denied, Clients.Caller);
            }
        }
        else
        {
            await SendMessagePacket("All Players are not ready", MessageType.Denied, Clients.Caller);
        }
    }

    private async Task<bool> NoLobbyFound(Lobby? lobby)
    {
        if (lobby != null) return false;
        await SendMessagePacket(caller: Clients.Caller, message: "This lobby does not exist",
            type: MessageType.Denied);
        return true;
    }


    private async Task<bool> NoPlayerFound(Player? player)
    {
        if (player != null) return false;
        await SendMessagePacket("The player doesn't exist in the lobby", MessageType.Denied, Clients.Caller);
        return true;
    }

    private async Task<bool> NoGameFound(Game? game)
    {
        if (game != null) return false;
        await SendMessagePacket("The lobby doesn't have an existing game", MessageType.Denied,
            Clients.Caller);
        return true;
    }

    public async Task LeaveBattle(byte lobbyId, byte participantId, byte battleId)
    {
        // Acknowledged
        await SendMessagePacket("Received leave Game request", MessageType.Acknowledged, Clients.Caller);

        var lobby = _server.GetLobby(lobbyId);

        if (lobby == null)
        {
            // TODO send message
            return;
        }

        var player = lobby.Players.FirstOrDefault((Player p) => p.ParticipantId == participantId);
        if (player == null)
        {
            // TODO send message
            return;
        }

        var game = lobby?.Game;
        if (game == null)
        {
            // TODO send message
            return;
        }

        var battle = game.GetBattle(battleId);
        if (battle == null)
        {
            // TODO send message
            return;
        }

        battle.PlayerForfeits(player);
        await SendMessagePacket("You have successfully left the battle", MessageType.Accepted, Clients.Caller);

        if (battle.Fighters.Select(fighter => fighter.PlayerId).Contains(participantId))
        {

        }
        else
        {
            await SendMessagePacket("Player wasn't found in requested battle", MessageType.Denied, Clients.Caller);
        }
    }

    public async Task RegisterPlayerTurn(byte lobbyId, byte playerId, byte battleId, string playerTurn)
    {
        // Acknowledged
        await SendMessagePacket("Received Start Game request", MessageType.Acknowledged, Clients.Caller);
        var lobby = _server.GetLobby(lobbyId);
        var game = lobby?.Game;
        //Check of a lobby has a game
        if (game != null)
        {
            //Get the battle player is in
            var battle = game.GetBattle(battleId);

            // var player = battle?.Fighters.Find(p => p.ParticipantId == playerId);
            var player = battle?.Fighters.FirstOrDefault(fighter => fighter.PlayerId == playerId);
            if (battle != null && player != null)
            {
                //Register players turn
                await SendMessagePacket("Your turn is executed", MessageType.Accepted, Clients.Caller);
            }
        }
        else
        {
            await SendMessagePacket("The lobby doesn't exist or the lobby doesn't have a game", MessageType.Denied,
                Clients.Caller);
        }
    }

    public async Task ExecuteBattleRound(byte lobbyId, byte playerId, byte battleId, string playerTurn)
    {
        // Acknowledged
        await SendMessagePacket("Received ExecuteBattle request", MessageType.Acknowledged, Clients.Caller);
        var lobby = _server.GetLobby(lobbyId);
        var game = lobby?.Game;
        //Check of a lobby has a game
        if (game != null)
        {
            //Get the battle player is in
            var battle = game.GetBattle(battleId);

            var player = battle?.Fighters.FirstOrDefault(fighter => fighter.PlayerId == playerId);
            if (battle != null && player != null)
            {
                /*battle.UpdateExecutedTurn(player);
                player.ExecuteTurn(playerTurn);
                battle.ExecutePlayerTurn(player);*/
                battle.ExecuteRound();
            }
        }
        else
        {
            await SendMessagePacket("The lobby doesn't exist or the lobby doesn't have a game", MessageType.Denied,
                Clients.Caller);
        }
    }

    public async Task ToggleIsPlayerReady(byte lobbyId, byte playerId, bool ready)
    {
        // Acknowledged
        await SendMessagePacket("Received player is Ready request", MessageType.Acknowledged, Clients.Caller);

        var lobby = _server.GetLobby(lobbyId);

        if (NoLobbyFound(lobby).Result)
        {
            return;
        }

        //Get player
        var player = lobby.Players.FirstOrDefault((Player p) => p.ParticipantId == playerId);

        if (player != null)
        {
            player.ReadyStatus = ready;
            //Send package
            // await Clients.Group($"{lobbyId}").ToggleReadyToStart(new PlayerReadyStatusPacket(ready, playerId));

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

        if (lobby?.Game != null)
        {
            //Set game settings to the new settings
            lobby.Game.Settings.settings = newGameSettings.settings;

            //send packet
            // await Clients.Group("lobbyId").ChangeGameSettings(new GameSettingsChangedPacket(newGameSettings.settings));
            await SendMessagePacket("You have successfully changed game settings", MessageType.Accepted,
                Clients.Caller);
        }
        else
        {
            await SendMessagePacket("The game doesn't exist", MessageType.Denied, Clients.Caller);
        }
    }

    public async Task ChangePlayerProfileInsideLobby(byte lobbyId, byte participantId, PlayerProfile newPlayerProfile)
    {
        await SendMessagePacket($"Request to change player profile from playerId: {participantId}, received",
            MessageType.Acknowledged,
            Clients.Caller);
        var lobby = _server.GetLobby(lobbyId);


        if (lobby == null)
        {
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
            return;
        }

        var player = lobby.Players.FirstOrDefault((Player p) => p.ParticipantId == participantId);

        if (player == null)
        {
            await SendMessagePacket("The player doesn't exist in the lobby", MessageType.Denied, Clients.Caller);
            return;
        }

        player.Profile = newPlayerProfile;

        // await Clients.OthersInGroup($"{lobbyId}")
        //     .PlayerProfileUpdated(
        //         new PlayerProfileUpdatedInLobbyPacket(participantId, player.DisplayName, newPlayerProfile));
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

        var player = lobby.Players.FirstOrDefault((Player p) => p.ParticipantId == playerId);
        if (player == null)
        {
            await SendMessagePacket("The player doesn't exist in the lobby", MessageType.Denied, Clients.Caller);
            return;
        }

        player.Role = newPlayerRole;

        await SendMessagePacket($"You have successfully changed role. New role: {newPlayerRole}", MessageType.Accepted,
            Clients.Caller);
        // await Clients.OthersInGroup($"{lobbyId}")
        //     .PlayerRoleChanged(new PlayerRoleChangedPacket(playerId, newPlayerRole));
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

    private byte GenerateBattleId(Game? game)
    {
        var battleIds = game?.Battles.Select(battle => battle.BattleId).ToHashSet() ?? [];

        byte battleId;
        do
        {
            battleId = (byte)_random.Next(1, 256);
        } while (!battleIds.Contains(battleId));

        return battleId;
    }


    private async Task SendMessagePacket(string message, MessageType type, IHubClient caller)
    {
        /*
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
        */
    }


    enum MessageType
    {
        Accepted,
        Denied,
        Acknowledged,
        Invalid
    }
}