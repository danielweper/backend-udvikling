using Turnbased_Game.Models.Packages;

namespace Turnbased_Game.Models.Client;

public class Client : IClient
{
    public event Func<byte, string>? ReceivedUserMessage;
    public event Func<string>? ReceivedSystemMessage;
    public event Func<byte, string>? ReceivedMessage;
    public void SendMessage(string message)
    {
        throw new NotImplementedException();
    }

    public event Func<bool>? BattleIsOver;
    public event Func<string>? TurnIsOver;
    public void SubmitTurn(string turn)
    {
        throw new NotImplementedException();
    }

    public event Func<string>? JoinedLobby;
    public event Func<string>? LeftLobby;
    public event Func<byte, IPlayerProfile>? PlayerJoined;
    public event Func<byte>? PlayerLeft;
    public event Func<ulong>? GameStarting;
    public event Func<IGameSettings>? GameSettingsChanged;
    public event Func<byte, IPlayerProfile>? PlayerChangedProfile;
    public event Func<byte, IRole>? PlayerChangedRole;
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

    public void HandleBadRequest(int requestId)
    {
        throw new NotImplementedException();
    }

    public event Func<byte, IRole>? RoleChangeRequested;
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

    public async void SendPackage(IPackage package)
    {
        Console.WriteLine(package.Package);
        // set LastPackageId to package.id
        LastPackageId = package.id;

    }
    public byte id { get; set; }
    public byte LastPackageId { get; private set; }
    public byte LobbyId { get; set; }
}