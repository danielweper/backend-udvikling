using Core.Model;
using Core.Packets;
using Core.Packets.Client;
using Core.Packets.Server;
using Core.Packets.Shared;
using Core.Packets.Transport;
using ServerLogic;

namespace ClientLogic;

public class Client : IClient
{
    public byte id { get; protected set; }
    public byte lobbyId { get; protected set; }
    public ClientStates CurrentState { get; protected set; }
    public PacketTransport Transporter { get; init; }
    protected IPacket? lastPackage = null;

    public Client(PacketTransport transporter)
    {
        Transporter = transporter;
        Transporter.PacketReceived += ReceivePackage;
        Transporter.OnConnected += delegate () { CurrentState |= ClientStates.IsConnected; };
        Transporter.OnDisconnected += delegate () { CurrentState &= ~ClientStates.IsConnected; };

        JoinedLobby += delegate (string s) { CurrentState |= ClientStates.IsInLobby; };
        LeftLobby += delegate (string s) { CurrentState &= ~ClientStates.IsInLobby; };
        GameStarting += delegate (ulong u) { CurrentState |= ClientStates.IsInGame; };

        ReceivedUserMessage += ReceivedMessage;
        ReceivedSystemMessage += (string content) => ReceivedMessage?.Invoke(0, content);

        if (Transporter.IsConnected)
        {
            CurrentState |= ClientStates.IsConnected;
        }
    }
    
    public event Action<byte, string>? ReceivedUserMessage;
    public event Action<string>? ReceivedSystemMessage;
    public event Action<byte, string>? ReceivedMessage;
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

    public bool IsHost => (this.id == 1);

    public void SendMessage(string message)
    {
        SendPackage(new SendMessagePacket(this.id, message));
    }

    public void SubmitTurn(string turn)
    {
        SendPackage(new SubmitTurnPacket(turn));
    }

    public void ListAvailableLobbies()
    {
        SendPackage(new ListAvailableLobbiesPacket());
    }

    public void JoinLobby(byte lobbyId)
    {
        SendPackage(new JoinLobbyPacket(lobbyId));
    }

    public void DisconnectLobby()
    {
        SendPackage(new DisconnectLobbyPacket());
        CurrentState = CurrentState & ~ClientStates.IsInLobby;
    }

    public void IsReady()
    {
        SendPackage(new ToggleReadyToStartPacket(true));
    }

    public void IsNotReady()
    {
        SendPackage(new ToggleReadyToStartPacket(false));
    }

    public void RequestProfileUpdate(IPlayerProfile profile)
    {
        SendPackage(new RequestPlayerUpdatePacket(profile));
    }

    public void RequestRoleChange(IRole role)
    {
        SendPackage(new RequestRoleChangePacket(role));
    }

    public void CreateLobby()
    {
        SendPackage(new CreateLobbyPacket());
        /*
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
        */
    }
    public void ChangeGameSettings(string settings)
    {
        if (!this.IsHost)
        {
            // TODO: error message?
            return;
        }

        SendPackage(new ChangeGameSettingsPacket(settings));
    }

    public void KickPlayer(byte playerId, string reason)
    {
        if (!this.IsHost)
        {
            // TODO: error message?
            return;
        }

        SendPackage(new KickPlayerPacket(playerId, reason));
    }

    public void StartGame()
    {
        if (!this.IsHost)
        {
            // TODO: error message?
            return;
        }

        SendPackage(new StartGamePacket());
        // TODO: act on the answer as well
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
        await Transporter.SendPacket(package);
        lastPackage = package;

    }
    public virtual async void ReceivePackage(IPacket package)
    {
        switch (package.Type)
        {
            case PacketType.Ping:
                SendPackage(new PingPacket());
                break;
            case PacketType.InvalidRequest:
                BadRequest?.Invoke();
                break;
            case PacketType.LobbyCreated:
                CurrentState |= ClientStates.IsInLobby | ClientStates.IsHost;
                break;
            case PacketType.LobbyInfo:
                string lobbyInfo = ((LobbyInfoPacket)package).Info;
                JoinedLobby?.Invoke(lobbyInfo);
                break;
            case PacketType.PlayerJoinedLobby:
                byte joinedId = ((PlayerJoinedLobbyPacket)package).PlayerId;
                IPlayerProfile joinedProfile = ((PlayerJoinedLobbyPacket)package).Profile;
                PlayerJoined?.Invoke(joinedId, joinedProfile);
                break;
            case PacketType.PlayerLeftLobby:
                byte leftId = ((PlayerLeftLobbyPacket)package).PlayerId;
                PlayerLeft?.Invoke(leftId);
                break;
            case PacketType.AvailableLobbies:
                break;
            case PacketType.GameStarting:
                GameStarting?.Invoke(0);
                break;
            case PacketType.GameSettingsChanged:
                IGameSettings newSettings = ((GameSettingsChangedPacket)package).NewSettings;
                GameSettingsChanged?.Invoke(newSettings);
                break;
            case PacketType.PlayerChanged:
                byte profileId = ((PlayerProfileChangedPacket)package).PlayerId;
                IPlayerProfile newProfile = ((PlayerProfileChangedPacket)package).UpdatedProfile;
                PlayerChangedProfile?.Invoke(profileId, newProfile);
                break;
            case PacketType.RoleChangeRequested:
                // TODO: give the user a choice?
                SendPackage(new AcceptedPacket((byte)PacketType.RoleChangeRequested));
                break;
            case PacketType.RoleChanged:
                byte roleId = ((RoleChangedPacket)package).PlayerId;
                IRole newRole = ((RoleChangedPacket)package).NewRole;
                PlayerChangedRole?.Invoke(roleId, newRole);
                break;
            case PacketType.ExecuteTurn:
                // TODO: fix
                TurnIsOver?.Invoke("TODO");
                break;
            case PacketType.BattleIsOver:
                // TODO: react on the content of the packet
                BattleIsOver?.Invoke(true);
                break;
            case PacketType.UserMessage:
                byte senderId = ((UserMessagePacket)package).SenderId;
                string userContent = ((UserMessagePacket)package).Content;
                ReceivedUserMessage?.Invoke(senderId, userContent);
                break;
            case PacketType.SystemMessage:
                string systemContent = ((SystemMessagePacket)package).Content;
                ReceivedSystemMessage?.Invoke(systemContent);
                break;
            case PacketType.RegisterPlayerTurn:
                break;
            case PacketType.PlayerReadyStatus:
                break;
            case PacketType.PlayerProfileCreated:
                break;
            case PacketType.Acknowledged:
            default:
                // ignore
                break;
        }
    }
}