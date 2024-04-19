using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Client;

public class Participant: IParticipant
{
    public byte id { get; }
    public event Func<string>? JoinedLobby;
    public event Func<byte>? LeftLobby;
    public event Func<byte, IPlayerProfile>? PlayerJoined;
    public event Func<byte>? PlayerLeft;
    public event Func<ulong>? GameStarting;
    public event Func<IGameSettings>? GameSettingsChanged;
    public event Func<byte, IPlayerProfile>? PlayerChangedProfile;
    public event Func<byte, IRole>? PlayerChangedRole;

    public Participant(byte id)
    {
        this.id = id;
    }
    public Task ReceiveAcknowledgePacket(Acknowledged packet)
    {
        throw new NotImplementedException();
    }

    public Task ReceiveAcceptedPacket(ReceiveMessagePacket packet)
    {
        throw new NotImplementedException();
    }

    public Task ReceivePacket(IClient packet)
    {
        throw new NotImplementedException();
    }

    public void ListAvailableLobbies()
    {
        throw new NotImplementedException();
    }

    public Task JoinLobbyRequest(LobbyInfoPacket content)
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
}