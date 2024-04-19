using Turnbased_Game.Models.Packets;
using Turnbased_Game.Models.Packets.Client;

namespace Turnbased_Game.Models.Client;

public class Client : IClient
{
    public event Action<byte, string>? ReceivedUserMessage;
    public event Action<string>? ReceivedSystemMessage;
    public event Action<byte, string>? ReceivedMessage;
    public event Action<IPackage>? ReceivedPackage; 
    public void SendMessage(string message)
    {
        SendMessage messagePacket = new SendMessage{
            senderId = this.id,
            message = message
        };

        SendPackage(messagePacket);
    }
    public event Action? BadRequest;  
    public event Action<bool>? BattleIsOver;
    public event Action<string>? TurnIsOver;
    public void SubmitTurn(string turn)
    {
        var packet = new SubmitTurn
        {
            turnInfo = turn
        };

        SendPackage(packet);
    }

    public event Action<string>? JoinedLobby;
    public event Action<string>? LeftLobby;
    public event Action<byte, IPlayerProfile>? PlayerJoined;
    public event Action<byte>? PlayerLeft;
    public event Action<ulong>? GameStarting;
    public event Action<IGameSettings>? GameSettingsChanged;
    public event Action<byte, IPlayerProfile>? PlayerChangedProfile;
    public event Action<byte, IRole>? PlayerChangedRole;

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
        throw new NotImplementedException();
    }

    public void RequestRoleChange(IRole role)
    {
        throw new NotImplementedException();
    }

    public event Action<byte, IRole>? RoleChangeRequested;
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
    }
    public void ChangeGameSettings(string settings)
    {
        ChangeGameSettings changeGameSettings = new ChangeGameSettings{
            settings = settings,
        };
        if (IsHost())
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
       KickPlayer kickPlayer = new KickPlayer{
           playerId = playerId,
           reason = reason
       };
       if (IsHost())
       { 
           SendPackage(kickPlayer);
       }
       else
       { 
           Console.WriteLine("Only host can kick players");
       }
    }

    public void CreateGame()
    {
        throw new NotImplementedException();
    }

    public void DeleteGame()
    {
        throw new NotImplementedException();
    }

    public void StartGame()
    {
        StartGame startGame = new StartGame();
        if (IsHost())
        {
            SendPackage(startGame);
        }
        else
        {
            Console.WriteLine("Only host can start game");
        }
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
        Console.WriteLine(package);
        // set LastPackageId to package.id
        lastPackage = package;

    }
    public virtual async void ReceivePackage(IPackage package)
    {
        Console.WriteLine(package);
    }

    private bool IsHost()
    {
        return this.id == 1;   
    }
    public byte id { get; set; }
    public IPackage lastPackage { get; protected set; }
    public byte lobbyId { get; set; }
}