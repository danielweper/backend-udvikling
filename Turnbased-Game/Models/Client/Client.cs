using Turnbased_Game.Models.Packages;
using Turnbased_Game.Models.Packages.Client;

namespace Turnbased_Game.Models.Client;

public class Client : IClient
{
    public event Action<byte, string>? ReceivedUserMessage;
    public event Action<string>? ReceivedSystemMessage;
    public event Action<byte, string>? ReceivedMessage;
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void JoinLobby(int lobbyId)
    {
        throw new NotImplementedException();
    }

    public void DisconnectLobby()
    {
        throw new NotImplementedException();
    }

    public void IsReady()
    {
        throw new NotImplementedException();
    }

    public void IsNotReady()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void ChangeGameSettings(string settings)
    {
        throw new NotImplementedException();
    }

    public void KickPlayer(int playerId, string reason)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
    public void ReceivePackage(IPackage package)
    {
        throw new NotImplementedException();
    }
    public byte id { get; set; }
    public IPackage lastPackage { get; protected set; }
    public byte lobbyId { get; set; }
}