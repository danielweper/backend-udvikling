using Turnbased_Game.Models.Packages.Client;
using Turnbased_Game.Models.Packages.Shared;

namespace Turnbased_Game.Models.Client;

public interface IParticipant
{
    public byte id { get;}
    public event Func<string> JoinedLobby;
    public event Func<byte> LeftLobby; // check
    public event Func<byte, IPlayerProfile> PlayerJoined;
    public event Func<byte> PlayerLeft;
    public event Func<ulong> GameStarting; // maybe DateTime instead of ulong
    public event Func<IGameSettings> GameSettingsChanged;
    public event Func<byte, IPlayerProfile> PlayerChangedProfile;
    public event Func<byte, IRole> PlayerChangedRole;
    Task ReceiveAcknowledgePacket(Acknowledged packet);
    Task ReceiveAcceptedPacket(ReceiveMessagePacket packet);
    Task ReceivePacket(IClient packet);
    public void ListAvailableLobbies();
    public Task JoinLobbyRequest(IJoinLobby content);
    public void DisconnectLobby();
    public void IsReady();
    public void IsNotReady();
    public void RequestProfileUpdate(IPlayerProfile profile);
    public void RequestRoleChange(IRole role);


}