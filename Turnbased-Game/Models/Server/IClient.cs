using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Server;
using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Server;

public interface IClient
{
    public Task ReceiveAcknowledgePacket(AcknowledgedPacket packet);
    Task ReceiveAcceptedPacket(AcceptedPacket packet);
    Task ReceiveDeniedPacket(DeniedPacket packet);
    public Task PlayerJoiningLobby(LobbyInfoPacket content);
    public Task PlayerHasJoined(PlayerJoinedLobbyPacket content);
    public Task ListAvailableLobbies(AvailableLobbiesPacket packet);
    public Task DisconnectLobby(byte playerId);
    public void IsReady();
    public void IsNotReady();
    public void RequestProfileUpdate(IPlayerProfile profile);
    public void RequestRoleChange(IRole role);
    public event Func<byte, IRole> RoleChangeRequested;

    public Task<byte> CreateLobbyRequest(CreateLobbyPacket packet);
    public void ChangeGameSettings(string settings); // JSON
    public Task KickPlayerRequest(int playerId, string reason);
    public Task CreateGame();
    public void DeleteGame();
    public void StartGame();
    public Task Denied(int requestId);
}