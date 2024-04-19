using Turnbased_Game.Models;
using Turnbased_Game.Models.Packets;
using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Transport;

namespace ClientLogic;

public class Client : IClient
{
    public byte id { get; protected set; }
    public byte lobbyId { get; protected set; }
    public ClientStates currentState { get; protected set; }
    protected IPackage? lastPackage = null;
    protected PacketTransport transporter;

    public Client(PacketTransport transporter)
    {
        this.transporter = transporter;
        this.transporter.PacketReceived += ReceivePackage;

        OnConnected += delegate () { currentState |= ClientStates.IsConnected; };
        JoinedLobby += delegate (string s) { currentState |= ClientStates.IsInLobby; };
        LeftLobby += delegate (string s) { currentState &= ~ClientStates.IsInLobby; };
        GameStarting += delegate (ulong u) { currentState |= ClientStates.IsInGame; };
    }

    public event Action? OnConnected;
    public event Action<byte, string>? ReceivedUserMessage;
    public event Action<string>? ReceivedSystemMessage;
    public event Action<byte, string>? ReceivedMessage;
    public event Action<IPackage>? ReceivedPackage;
    public event Action? BadRequest;
    public event Action<bool>? BattleIsOver;
    public event Action<string>? TurnIsOver;
    public event Action<string>? JoinedLobby;
    public event Action<string>? LeftLobby;
    public event Action<byte, IPlayerProfile>? PlayerJoined;
    public event Action<byte>? PlayerLeft;
    public event Action<ulong>? GameStarting;
    public event Action<IGameSettings>? GameSettingsChanged;
    public event Action<byte, IPlayerProfile>? PlayerChangedProfile;
    public event Action<byte, IRole>? PlayerChangedRole;
    public event Action<byte, IRole>? RoleChangeRequested;

    public bool isHost => (this.id == 1);

    public void SendMessage(string message)
    {
        SendMessage messagePacket = new SendMessage{
            senderId = this.id,
            message = message
        };

        SendPackage(messagePacket);
    }

    public void SubmitTurn(string turn)
    {
        var packet = new SubmitTurn
        {
            turnInfo = turn
        };

        SendPackage(packet);
    }

    public void ListAvailableLobbies()
    {
        var packet = new ListAvailableLobbies();
        SendPackage(packet);
    }

    public void JoinLobby(byte lobbyId)
    {
        var packet = new JoinLobby
        {
            lobbyId = lobbyId
        };

        SendPackage(packet);
    }

    public void DisconnectLobby()
    {
        var packet = new DisconnectLobby();
        SendPackage(packet);
        currentState = currentState & ~ClientStates.IsInLobby;
    }

    public void IsReady()
    {
        var packet = new ToggleReadyToStart
        {
            newStatus = true,
        };

        SendPackage(packet);
    }

    public void IsNotReady()
    {
        var packet = new ToggleReadyToStart
        {
            newStatus = false,
        };

        SendPackage(packet);
    }

    public void RequestProfileUpdate(IPlayerProfile profile)
    {
        var packet = new RequestPlayerUpdate
        {
            newProfile = profile,
        };

        SendPackage(packet);
    }

    public void RequestRoleChange(IRole role)
    {
        var packet = new RequestRoleChange
        {
            newRole = role,
        };

        SendPackage(packet);
    }

    public void CreateLobby()
    {
        CreateLobby createLobby = new CreateLobby();
        SendPackage(createLobby);
        ReceivedPackage += (package) =>
        {
            if (package is IAccepted)
            {
                Console.WriteLine("Lobby was succesfully created");
            }
            else if (package is IDenied)
            {
                Console.WriteLine("Lobby creation faield");
            }
        };

        // TODO: act on the answer as well
        currentState = currentState | ClientStates.IsInLobby | ClientStates.IsHost;
    }
    public void ChangeGameSettings(string settings)
    {
        ChangeGameSettings changeGameSettings = new ChangeGameSettings
        {
            settings = settings,
        };
        if (this.isHost)
        {
            SendPackage(changeGameSettings);
        }
        else
        {
            Console.WriteLine("Only host can change settings");
        }
    }

    public void KickPlayer(byte playerId, string reason)
    {
       KickPlayer kickPlayer = new KickPlayer
       {
           playerId = playerId,
           reason = reason
       };
       if (this.isHost)
       { 
           SendPackage(kickPlayer);
       }
       else
       { 
           Console.WriteLine("Only host can kick players");
       }
    }

    public void CreateGame(string gameName)
    {
        CreateGame createGame = new CreateGame
        {
            gameName = gameName
        };
        SendPackage(createGame);
    }

    public void DeleteGame(string gameName)
    {
        DeleteGame deleteGame = new DeleteGame
        {
            gameName = gameName
        };
        SendPackage(deleteGame);
    }

    public void StartGame()
    {
        StartGame startGame = new StartGame();
        if (this.isHost)
        {
            SendPackage(startGame);
        }
        else
        {
            Console.WriteLine("Only host can start game");
        }

        // TODO: act on the answer as well
        currentState |= ClientStates.IsInGame;
    }

    public void Accepted(int requestId)
    {
        throw new NotImplementedException();
    }

    public void Denied(int requestId)
    {
        throw new NotImplementedException();
    }

    public virtual async void SendPackage(IPackage package)
    {
        transporter.SendPacket(package);
        // set LastPackageId to package.id
        lastPackage = package;

    }
    public virtual async void ReceivePackage(IPackage package)
    {
        Console.WriteLine(package);
    }
}