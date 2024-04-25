using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Server;
using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Server;

public interface IClient
{
    public Task ReceiveAcknowledgePacket(AcknowledgedPacket packet);
    Task ReceiveAcceptedPacket(AcceptedPacket packet);
    Task ReceiveDeniedPacket(DeniedPacket packet);
    public Task ReceiveInvalidPacket(InvalidPacket packet);
    public Task PlayerJoiningLobby(LobbyInfoPacket content);
    public Task PlayerHasJoined(PlayerJoinedLobbyPacket content);
    public Task ListAvailableLobbiesRequest(AvailableLobbiesPacket packet);
    public Task PlayerHasLeft(PlayerLeftLobbyPacket playerLeftLobbyPacket);

    public Task ToggleReadyToStart(PlayerReadyStatusPacket playerReadyStatusPacket);

    public Task PlayerProfileUpdated(PlayerProfileChangedPacket profile);

    public Task PlayerRoleChanged(PlayerRoleChangedPacket packet);
    public void RequestRoleChange(IRole role);
    public event Func<byte, IRole> RoleChangeRequested;

    public Task<byte> CreateLobbyRequest(CreateLobbyPacket packet);
    public Task ChangeGameSettings(GameSettingsChangedPacket packet); // JSON
    public Task PlayerKicked(KickPlayerPacket packet);
    public Task GameCreated(GameInfoPacket packet);
    public Task DeleteGame(DeleteGamePacket packet);
    public Task StartGame(GameStartingPacket packet);
    public Task SubmitTurn(ExecuteTurnPacket packet);
    public Task IsBattleOver(BattleIsOverPacket packet);
}