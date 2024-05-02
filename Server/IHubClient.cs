using Core.Packets.Shared;
using Core.Packets.Server;
using Core.Packets;

namespace ServerLogic;

public interface IHubClient
{
    public Task ReceiveAcknowledgePacket(AcknowledgedPacket packet);
    Task ReceiveAcceptedPacket(AcceptedPacket packet);
    Task ReceiveDeniedPacket(DeniedPacket packet);
    public Task ReceiveInvalidPacket(InvalidRequestPacket packet);
    public Task PlayerJoiningLobby(LobbyInfoPacket content);
    public Task PlayerHasJoined(PlayerJoinedLobbyPacket content);
    public Task ListAvailableLobbiesRequest(AvailableLobbiesPacket packet);
    public Task PlayerHasLeft(PlayerLeftLobbyPacket playerLeftLobbyPacket);

    public Task ToggleReadyToStart(bool newStatus);

    public Task PlayerProfileUpdated(PlayerProfileChangedPacket profile);
    public Task PlayerRoleChanged(RoleChangedPacket packet);
    public Task ChangeGameSettings(GameSettingsChangedPacket packet); // JSON

    public Task PlayerLeft(PlayerLeftLobbyPacket packet);
    public Task GameCreated(LobbyInfoPacket packet);
    public Task StartGame(GameStartingPacket packet);
    public Task IsBattleOver(BattleOverPacket packet);
    public Task ExecuteRound(ExecuteTurnPacket packet);

    public Task ReceivePacket(IPacket packet);
}