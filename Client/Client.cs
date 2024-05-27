using Core.Model;
using Core.Packets;
using Core.Packets.Client;
using Core.Packets.Server;
using Core.Packets.Shared;
using Core.Packets.Transport;
using ServerLogic;
using System.Reflection.PortableExecutable;

namespace ClientLogic;

public class Client : IClient
{
    public byte? lobbyId { get; protected set; }
    public ClientStates CurrentState { get; protected set; }
    public PacketTransport Transporter { get; init; }
    protected IPacket? lastPackage = null;

    protected List<string> players = new();

    public Client(PacketTransport transporter)
    {
        Transporter = transporter;
        Transporter.PacketReceived += ReceivePackage;
        Transporter.OnConnected += delegate() { CurrentState |= ClientStates.IsConnected; };
        Transporter.OnDisconnected += delegate() { CurrentState &= ~ClientStates.IsConnected; };

        JoinedLobby += delegate(string lobbyInfo)
        {
            CurrentState |= ClientStates.IsInLobby;
            var info = ParseLobbyInfo(lobbyInfo);
            lobbyId = info.Item1;
            players = info.Item3.ToList();
        };
        LobbyChanged += delegate (string lobbyInfo)
        {
            var info = ParseLobbyInfo(lobbyInfo);
            string host = info.Item2;
            players = info.Item3.ToList();

            if (host == Name)
            {
                CurrentState |= ClientStates.IsHost;
            }
            // byte id,
            //     Player host,
            // Player[] players,
            // int maxPlayerCount,
            //     LobbyVisibility lobbyVisibility,
            // GameInfo? gameInfo)
        };
        LeftLobby += delegate(string s)
        {
            CurrentState &= ClientStates.IsConnected;
            players.Clear();
        };
        GameStarting += delegate(DateTime u) { CurrentState |= (ClientStates.IsInGame | ClientStates.IsFighter); };
        BattleIsOver += delegate (bool b) { CurrentState &= ~(ClientStates.IsInGame | ClientStates.IsFighter); };

        ReceivedUserMessage += (string sender, string content) => ReceivedMessage?.Invoke(sender, content);
        ReceivedSystemMessage += (string content) => ReceivedMessage?.Invoke("Server", content);

        if (Transporter.IsConnected)
        {
            CurrentState |= ClientStates.IsConnected;
        }

        _name = "";
    }

    public event Action<string, string>? ReceivedUserMessage;
    public event Action<string>? ReceivedSystemMessage;
    public event Action<string, string>? ReceivedMessage;
    public event Action? BadRequest;
    public event Action<bool>? BattleIsOver;
    public event Action<string>? TurnIsOver;
    public event Action<string>? JoinedLobby;
    public event Action<string>? LobbyChanged;
    public event Action<string>? LeftLobby;
    public event Action<byte, IPlayerProfile>? PlayerJoined;
    public event Action<string>? PlayerLeft;
    public event Action<DateTime>? GameStarting;
    public event Action<IGameSettings>? GameSettingsChanged;
    public event Action<byte, IPlayerProfile>? PlayerChangedProfile;
    public event Action<byte, IRole>? PlayerChangedRole;
    public event Action<byte, IRole>? RoleChangeRequested;
    public event Action<bool> PlayerStatusChanged;
    public event Action<string>? ListingLobbies;

    public bool IsHost => (CurrentState.HasFlag(ClientStates.IsHost));
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (CurrentState.HasFlag(ClientStates.IsInLobby))
            {
                return;
            }

            _name = value;
        }
    }

    public void SendMessage(string message)
    {
        SendPackage(new SendMessagePacket(message));
    }

    public void SubmitTurn(char turn)
    {
        SendPackage(new SubmitTurnPacket(turn));
    }

    public void ListAvailableLobbies()
    {
        SendPackage(new ListAvailableLobbiesPacket());
    }

    public void JoinLobby(byte lobbyId)
    {
        SendPackage(new JoinLobbyPacket(lobbyId, Name));
    }

    public void DisconnectLobby()
    {
        SendPackage(new DisconnectLobbyPacket(lobbyId.Value));
        LeftLobby?.Invoke(Name);
    }

    public void IsReady()
    {
        SendPackage(new ToggleReadyToStartPacket(lobbyId.Value, true));
    }

    public void IsNotReady()
    {
        SendPackage(new ToggleReadyToStartPacket(lobbyId.Value, false));
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
        Console.WriteLine($"Create lobby name: {Name}");
        SendPackage(new CreateLobbyPacket(Name));
        CurrentState |= ClientStates.IsHost;
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

    public void KickPlayer(string kickPlayerName, string reason)
    {
        if (!IsHost)
        {
            // TODO: error message?
            return;
        }

        SendPackage(new KickPlayerPacket(kickPlayerName, reason, lobbyId.Value));
    }

    public void StartGame()
    {
        /*if (!this.IsHost)
        {
            // TODO: error message?
            return;
        }*/
        SendPackage(new StartGamePacket(lobbyId.Value, DateTime.Now));
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
            case PacketType.Denied:
            {
                BadRequest?.Invoke();
                break;
            }
            
            case PacketType.LobbyCreated:
                CurrentState |= ClientStates.IsInLobby | ClientStates.IsHost;
                break;
            case PacketType.LobbyInfo:
                string lobbyInfo = ((LobbyInfoPacket)package).Info;
                JoinedLobby?.Invoke(lobbyInfo);
                break;
            case PacketType.LobbyChanged:
                string lobbyChanged = ((LobbyChangedPacket)package).Info;
                JoinedLobby?.Invoke(lobbyChanged);
                break;
            case PacketType.PlayerJoinedLobby:
                byte joinedId = ((PlayerJoinedLobbyPacket)package).PlayerId;
                IPlayerProfile joinedProfile = ((PlayerJoinedLobbyPacket)package).Profile;
                PlayerJoined?.Invoke(joinedId, joinedProfile);
                break;
            case PacketType.PlayerLeftLobby:
                string displayName = ((PlayerLeftLobbyPacket)package).DisplayName;
                if (displayName == Name)
                {
                    LeftLobby?.Invoke(displayName);
                }
                else
                {
                    PlayerLeft?.Invoke(displayName);
                }
                break;
            case PacketType.AvailableLobbies:
                string lobbyInfos = ((AvailableLobbiesPacket)package).LobbyInfo;
                ListingLobbies?.Invoke(lobbyInfos);
                break;
            case PacketType.KickPlayer:
                string leavingPlayer = ((KickPlayerPacket)package).KickPlayerName;
                LeftLobby?.Invoke(leavingPlayer);
                break;
            case PacketType.PlayerKicked:
                LeftLobby?.Invoke("Placeholder");
                    break;
            case PacketType.GameStarting:
                var gameStartingPacket = (GameStartingPacket)package;
                GameStarting?.Invoke(gameStartingPacket.Startingtime);
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
                string senderName = ((UserMessagePacket)package).SenderName;
                string userContent = ((UserMessagePacket)package).Content;
                ReceivedUserMessage?.Invoke(senderName, userContent);
                break;
            case PacketType.SystemMessage:
                string systemContent = ((SystemMessagePacket)package).Content;
                ReceivedSystemMessage?.Invoke(systemContent);
                break;
            case PacketType.RegisterPlayerTurn:
                break;
            case PacketType.ToggleReadyToStart:
                var isReady = ((ToggleReadyPacket)package).NewStatus;
                PlayerStatusChanged?.Invoke(isReady);
                break;
            case PacketType.PlayerProfileCreated:
                break;
            case PacketType.Acknowledged:
            case PacketType.Accepted:
                break;
            default:
                Console.WriteLine($"Received {package.Type}, but not yet implemented in Client");
                break;
        }
    }

    public static (byte, string, string[], int, LobbyVisibility) ParseLobbyInfo(string lobbyInfo)
    {
        var reader = new StringReader(lobbyInfo);
        byte lobbyId = byte.Parse(reader.ReadLine()!);
        string host = reader.ReadLine()!.Trim();
        string[] players = reader.ReadLine()!.Trim().Split(", ");
        int maxPlayers = int.Parse(reader.ReadLine().Split(": ")[1].Trim()!);

        return (lobbyId, host, players, maxPlayers, LobbyVisibility.Public);
    }
}