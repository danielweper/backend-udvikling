using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using ServerLogic;
using ServerLogic.Model.Fighting;
using Core.Model;
using Core.Packets;
using Turnbased_Game.Models;

namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IHubClient>
{
    private const GameType DefaultGameType = GameType.RoundRobin;
    private const string DefaultName = "Joe";


    private readonly Random _random = new();

    public override async Task OnConnectedAsync()
    {
        ConnectionKnower.AddConnection(Context.ConnectionId);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        ConnectionKnower.RemoveConnection(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task CreatePlayerProfile(string name, Color color = Color.Red)
    {
        if (name.Length > 30)
        {
            await SendMessagePacket("This name is not Valid, please choose a name that is less than 30 letters",
                MessageType.Denied, Clients.Caller);
            return;
        }

        if (name.Any(c => !char.IsLetter(c)))
        {
            await SendMessagePacket(
                "This name is not Valid, please choose a name that doesn't contains ',' or new line",
                MessageType.Denied, Clients.Caller);
            return;
        }

        PlayerProfile playerProfile = new(color, name);
        await SendMessagePacket(
            $"You have successfully created a playerProfile with Color: {color.ToString()}, Name: {name}",
            MessageType.Accepted, Clients.Caller);
        //await Clients.Caller.PlayerProfileCreated(new PlayerProfileCreatedPacket(playerProfile));
    }

    public PlayerProfile DefaultPlayerProfile(string connectionId)
    {
        return new PlayerProfile(Color.Red, DefaultName);
    }

    public async Task CreateLobby(int maxPlayerCount, LobbyVisibility lobbyVisibility,
        PlayerProfile? playerProfile = null)
    {
        Console.WriteLine("Someone wants to create a lobby");
        playerProfile ??= DefaultPlayerProfile(Context.ConnectionId);

        // Create host
        var host = new Player(playerProfile.Name, 1, playerProfile);

        // Create lobby
        byte lobbyId = GenerateLobbyId();
        Lobby lobby = new(lobbyId, host, new Game(DefaultGameType), maxPlayerCount, lobbyVisibility);
        Server.AddLobby(lobby);
        await Clients.Caller.LobbyCreated(lobbyId);
        //Send the playerId to client
        await Clients.Caller.PlayerJoinedLobby(host.ParticipantId, "playerProfile");

        var lobbyInfo = lobby.GetInfo().ToString();
        await Clients.Caller.LobbyInfo(lobbyInfo);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");

        Console.WriteLine($"Someone created a lobby {lobbyId}");
        Console.WriteLine($"New Lobby Count: {Server._lobbies.Count}");
        ConnectionKnower.MakePlayerConnection(Context.ConnectionId, host, lobby);
    }

    public async Task JoinLobby(byte lobbyId, PlayerProfile playerProfile)
    {
        Console.WriteLine($"Someone wants to join the lobby {lobbyId}");
        Console.WriteLine($"Lobby Count: {Server._lobbies.Count}");
        foreach (Lobby l in Server._lobbies)
        {
            Console.WriteLine($"[LOBBY] {l.Id}");
        }

        var lobby = Server.GetLobby(lobbyId);
        Console.WriteLine($"Found '{lobby}'");

        if (lobby == null)
        {
            await Clients.Caller.InvalidRequest((byte)PacketType.JoinLobby, "No lobby found with corresponding id");
            return;
        }

        if (lobby.IsFull)
        {
            await Clients.Caller.Denied((byte)PacketType.JoinLobby);
            return;
        }

        HashSet<string> playerNames = lobby.Players.Select(pl => pl.DisplayName).ToHashSet();
        string displayName = GetDisplayName(playerProfile.Name, playerNames);
        Player player = new(displayName, 1, playerProfile);

        Console.WriteLine($"player joined starts with id: {player.ParticipantId}");
        lobby.AddPlayer(player);
        Console.WriteLine($"player joined has id: {player.ParticipantId}");

        LobbyInfo lobbyInfo = new(lobbyId, lobby.Host, lobby.Players.ToArray(),
            lobby.MaxPlayerCount, lobby.Visibility, lobby.Game?.GetInfo());
        await Clients.Caller.LobbyInfo(lobbyInfo.ToString());
        await Clients.Group($"{lobbyId}").PlayerJoinedLobby(player.ParticipantId, "profile");

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        Console.WriteLine($"{player.DisplayName} joined the lobby '{lobbyId}'");
        ConnectionKnower.MakePlayerConnection(Context.ConnectionId, player, lobby);
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
        var lobby = Server.GetLobby(lobbyId);

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

    public async Task LeaveLobby(byte lobbyId)
    {
        var lobby = Server.GetLobby(lobbyId);
        byte? playerId = ConnectionKnower.GetPlayer(Context.ConnectionId)?.ParticipantId;
        if (lobby == null || !playerId.HasValue)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "This lobby does not exist",
                type: MessageType.Denied);
            return;
        }

        Player? player = lobby.Players.FirstOrDefault((Player p) => p.ParticipantId == playerId);
        if (player == null)
        {
            await SendMessagePacket(caller: Clients.Caller, message: "You are not in this lobby",
                type: MessageType.Denied);
            return;
        }

        //Remove Player
        lobby.RemovePlayer(player);

        // Finds the connection id for the specified player
        ConnectionKnower.MakeNonPlayerConnection(Context.ConnectionId);

        // Removes player from SignalR group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{lobbyId}");

        //Check if player is the last player left in lobby
        if (lobby.IsEmpty)
        {
            Server.RemoveLobby(lobby);
        }
        else
        {
            //Send packet that a player left
            await Clients.Group($"{lobbyId}").PlayerLeftLobby(playerId.Value);
        }

        //Send packet to the player, that they have disconnect the lobby
        await Clients.Caller.Acknowledged(); // maybe accepted?
    }

    public async Task ListAvailableLobbies()
    {
        await SendMessagePacket("Received view AvailableLobbies", MessageType.Acknowledged,
            Clients.Caller); // Acknowledged

        //Get all the Lobbies from server
        List<LobbyInfo> lobbiesInfo = Server.GetAvailableLobbies();

        // AvailableLobbiesPacket availableLobbiesPacketPacket = new AvailableLobbiesPacket(lobbiesInfo);
        //Send the packet to the client

        StringBuilder stringBuilder = new();

        if (lobbiesInfo.Count == 0)
        {
            stringBuilder.AppendLine("No lobbies available to join");
            stringBuilder.AppendLine();
        }
        else
        {
            foreach (var lobbyInfo in lobbiesInfo)
            {
                // stringBuilder.AppendLine(lobbyInfo.ToString());

                stringBuilder.AppendLine($"Lobby Id: {lobbyInfo.id}");
                stringBuilder.AppendLine($"Host: {lobbyInfo.host.DisplayName}");
                stringBuilder.AppendLine($"Players in lobby: {lobbyInfo.players.Length}/{lobbyInfo.maxPlayer}");
                if (lobbyInfo.gameInfo is not null)
                {
                    // todo - gameinfo ???
                    stringBuilder.AppendLine($"GameType: {lobbyInfo.gameInfo.Value.GameSettings.GameType.ToString()}");
                    var battleHasStartedText = lobbyInfo.gameInfo.Value.BattleHasStarted
                        ? "The game is currently in progress"
                        : "The game has not started yet";

                    stringBuilder.AppendLine(battleHasStartedText);
                }

                stringBuilder.AppendLine();
            }
        }

        await Clients.Caller.AvailableLobbies(stringBuilder.ToString());
        Console.WriteLine("Listing available lobbies for someone");
    }

    /*public async Task CreateGame(byte lobbyId, GameType gameType = DefaultGameType)
    {
        // Acknowledged
        await SendMessagePacket("Received CreateGame request", MessageType.Acknowledged, Clients.Caller);

        var lobby = Server.GetLobby(lobbyId);


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
    }*/

    //Start the Game(battle/battles)
    public async Task StartGame(byte lobbyId)
    {
        Console.WriteLine("Someone want to start the game");
        var lobby = Server.GetLobby(lobbyId);

        var allPlayersReady = lobby != null && lobby.Players.All(p => p.ReadyStatus);
        if (!allPlayersReady)
        {
            Console.WriteLine("All Players are not ready");
            return;
        }

        Console.WriteLine("All players are ready");
        //Get all Fighters 
        var fighters = lobby?.Players.Where(p => p.Role == PlayerRole.Fighter).ToList();

        //Check of a lobby has a game
        var game = lobby?.Game;
        if (fighters == null || game == null)
        {
            Console.WriteLine("The game or fighters do not exist");
            return;
        }

        //Check if there is even number of Fighters
        if (fighters.Count % 2 != 0)
        {
            Console.WriteLine("Game cannot start, need even number of Fighters");
            return;
        }

        // Game in progress status:
        lobby.Game.BattlesHasStarted = true;

        for (var i = 0; i < fighters.Count; i += 2)
        {
            //Create battle
            var battle = new Battle(GenerateBattleId(game), fighters[i], fighters[i + 1]);

            //Add battle to game
            game.Battles.Add(battle);
        }

        //Send packet to client
        await Clients.Group($"{lobbyId}").GameStarting(lobbyId, DateTime.Now);
        Console.WriteLine("game Started");
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

    public async Task LeaveBattle(byte lobbyId, byte battleId)
    {
        // Acknowledged
        await SendMessagePacket("Received leave Game request", MessageType.Acknowledged, Clients.Caller);

        var lobby = Server.GetLobby(lobbyId);
        var participantId = ConnectionKnower.GetPlayer(Context.ConnectionId)?.ParticipantId;

        if (lobby == null || participantId == null)
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

        if (battle.Fighters.Select(fighter => fighter.PlayerId).Contains(participantId.Value))
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
        var lobby = Server.GetLobby(lobbyId);
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

    public async Task ExecuteBattleRound(byte lobbyId, byte battleId, string playerTurn)
    {
        // Acknowledged
        await SendMessagePacket("Received ExecuteBattle request", MessageType.Acknowledged, Clients.Caller);
        var lobby = Server.GetLobby(lobbyId);
        var game = lobby?.Game;
        var playerId = ConnectionKnower.GetPlayer(Context.ConnectionId)?.ParticipantId;

        //Check of a lobby has a game
        if (game == null || playerId == null)
        {
            await SendMessagePacket("The lobby doesn't exist or the lobby doesn't have a game", MessageType.Denied,
                Clients.Caller);
            return;
        }

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

    public async Task ToggleIsPlayerReady(byte lobbyId, bool ready)
    {
        Console.WriteLine("Someone wants to be ready to start game");

        var lobby = Server.GetLobby(lobbyId);
        var playerId = ConnectionKnower.GetPlayer(Context.ConnectionId)?.ParticipantId;

        if (NoLobbyFound(lobby).Result)
        {
            Console.WriteLine("No Lobby found");
            return;
        }

        //Get player
        var player = lobby.Players.FirstOrDefault(p => p.ParticipantId == playerId);

        if (player == null)
        {
            Console.WriteLine("You are not in this lobby");
            return;
        }

        player.ReadyStatus = ready;
        //Send package

        // todo
        if (ready)
        {
            Console.WriteLine("You are ready");
            // await Clients.Group($"{lobby.Id}").ToggleReadyToStart(lobbyId, ready);
            Console.WriteLine("Your status is changed to ready");
        }
        else
        {
            Console.WriteLine("You are not ready");
            // await Clients.Group($"{lobby.Id}").ToggleReadyToStart(lobbyId, false);
            Console.WriteLine("Your status is changed to false");
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

    public async Task ChangePlayerProfileInsideLobby(byte lobbyId, PlayerProfile newPlayerProfile)
    {
        var participantId = ConnectionKnower.GetPlayer(Context.ConnectionId)?.ParticipantId;
        await SendMessagePacket($"Request to change player profile from playerId: {participantId}, received",
            MessageType.Acknowledged,
            Clients.Caller);
        var lobby = Server.GetLobby(lobbyId);


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
        var lobby = Server.GetLobby(lobbyId);


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

//Messages
    public async Task SendMessage(string message)
    {
        //If they doesn't send playerId - Maybe
        //var playerId = GetPlayerId(Context.ConnectionId);
        var player = ConnectionKnower.GetPlayer(Context.ConnectionId)!;
        var playerId = player.ParticipantId;

        Console.WriteLine($"{playerId} wants to send a message '{message}'");

        var lobby = Server._lobbies.Find(lobby => lobby.Players.Any(player => player.ParticipantId == playerId));
        if (lobby == null)
        {
            Console.WriteLine("lobby doesn't exist");
            await SendMessagePacket("The lobby doesn't exist", MessageType.Denied, Clients.Caller);
            return;
        }

        await Clients.Group($"{lobby.Id}").UserMessage(player.DisplayName, message);

        Console.WriteLine($"Message sent to the lobby {lobby.Id}");
    }

    private byte GenerateLobbyId()
    {
        byte lobbyId;
        do
        {
            lobbyId = (byte)_random.Next(1, 256);
        } while (!Server.LobbyIdIsFree(lobbyId));

        return lobbyId;
    }

    private byte GenerateParticipantId(Lobby? lobby)
    {
        HashSet<byte> usedIds = lobby?.Players.Select(p => p.ParticipantId).ToHashSet() ?? new HashSet<byte>();

        byte participantId;
        do
        {
            participantId = (byte)_random.Next(1, 256);
        } while (usedIds.Contains(participantId));

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