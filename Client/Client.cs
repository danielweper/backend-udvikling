using Core.Model;
using Core.Packets;
using Core.Packets.Client;
using Core.Packets.Shared;
using Core.Packets.Transport;

namespace ClientLogic;

public class Client : IClient
{
    public byte id { get; protected set; }
    public byte lobbyId { get; protected set; }
    public ClientStates currentState { get; protected set; }
    protected IPacket? lastPackage = null;
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
    public event Action<IPacket>? ReceivedPackage;
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
        var messagePacket = new SendMessagePacket
        {
            senderId = this.id,
            message = message
        };

        SendPackage(messagePacket);
    }

    public void SubmitTurn(string turn)
    {
        var packet = new SubmitTurnPacket
        {
            turnInfo = turn
        };

        SendPackage(packet);
    }

    public void ListAvailableLobbies()
    {
        var packet = new ListAvailableLobbiesPacket();
        SendPackage(packet);
    }

    public void JoinLobby(byte lobbyId)
    {
        var packet = new JoinLobbyPacket
        {
            lobbyId = lobbyId
        };

        SendPackage(packet);
    }

    public void DisconnectLobby()
    {
        var packet = new DisconnectLobbyPacket();
        SendPackage(packet);
        currentState = currentState & ~ClientStates.IsInLobby;
    }

    public void IsReady()
    {
        var packet = new ToggleReadyToStartPacket
        {
            newStatus = true,
        };

        SendPackage(packet);
    }

    public void IsNotReady()
    {
        var packet = new ToggleReadyToStartPacket
        {
            newStatus = false,
        };

        SendPackage(packet);
    }

    public void RequestProfileUpdate(IPlayerProfile profile)
    {
        var packet = new RequestPlayerUpdatePacket
        {
            newProfile = profile,
        };

        SendPackage(packet);
    }

    public void RequestRoleChange(IRole role)
    {
        var packet = new RequestRoleChangePacket
        {
            newRole = role,
        };

        SendPackage(packet);
    }

    public void CreateLobby()
    {
        var createLobby = new CreateLobbyPacket();
        SendPackage(createLobby);
        ReceivedPackage += (package) =>
        {
            if (package is AcceptedPacket)
            {
                Console.WriteLine("Lobby was succesfully created");
            }
            else
            {
                Console.WriteLine("Lobby creation faield");
            }
        };

        // TODO: act on the answer as well
        currentState = currentState | ClientStates.IsInLobby | ClientStates.IsHost;
    }
    public void ChangeGameSettings(string settings)
    {
        var changeGameSettings = new ChangeGameSettingsPacket
        {
            settings = settings,
        };
        if (this.isHost)
        {
            SendPackage(changeGameSettings);
        }
        else
        {
            //TODO: not a writeline
            Console.WriteLine("Only host can change settings");
        }
    }

    public void KickPlayer(byte playerId, string reason)
    {
       var kickPlayer = new KickPlayerPacket
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

    public void StartGame()
    {
        var startGame = new StartGamePacket();
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

    public virtual async void SendPackage(IPacket package)
    {
        transporter.SendPacket(package);
        // set LastPackageId to package.id
        lastPackage = package;

    }
    public virtual async void ReceivePackage(IPacket package)
    {
        Console.WriteLine(package);
    }
}