namespace Turnbased_Game.Models.Client;

public interface IParticipant
{
    public event Func<string> JoinedLobby;
    public event Func<string> LeftLobby;
    public event Func<byte, IPlayerProfile> PlayerJoined;
    public event Func<byte> PlayerLeft;
    public event Func<ulong> GameStarting;  // maybe DateTime instead of ulong
    public event Func<IGameSettings> GameSettingsChanged;
    public event Func<byte, IPlayerProfile> PlayerChangedProfile;
    public event Func<byte, IRole> PlayerChangedRole;

    public void ListAvailableLobbies();
    public void JoinLobby(int lobbyId);
    public void DisconnectLobby();
    public void IsReady();
    public void IsNotReady();
    public void RequestProfileUpdate(IPlayerProfile profile);
    public void RequestRoleChange(IRole role);
    public void HandleBadRequest(int requestId);
}