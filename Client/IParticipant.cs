using Core.Model;

namespace ClientLogic;

public interface IParticipant
{
    public event Action<string> JoinedLobby;
    public event Action<string> LeftLobby;
    public event Action<byte, IPlayerProfile> PlayerJoined;
    public event Action<string> PlayerLeft;
    public event Action<DateTime>? GameStarting;  // maybe DateTime instead of ulong
    public event Action<IGameSettings> GameSettingsChanged;
    public event Action<byte, IPlayerProfile> PlayerChangedProfile;
    public event Action<byte, IRole> PlayerChangedRole;
    public event Action BadRequest; 

    public void ListAvailableLobbies();
    public void JoinLobby(byte lobbyId);
    public void DisconnectLobby();
    public void IsReady();
    public void IsNotReady();
    public void RequestProfileUpdate(IPlayerProfile profile);
    public void RequestRoleChange(IRole role);
}