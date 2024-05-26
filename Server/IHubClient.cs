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

    public Task PlayerProfileUpdated(PlayerProfileChangedPacket profile);
    public Task PlayerRoleChanged(RoleChangedPacket packet);
    public Task ChangeGameSettings(GameSettingsChangedPacket packet); // JSON

    public Task PlayerLeft(PlayerLeftLobbyPacket packet);
    public Task GameCreated(LobbyInfoPacket packet);
    public Task StartGame(GameStartingPacket packet);
    public Task IsBattleOver(BattleOverPacket packet);
    public Task ExecuteRound(ExecuteTurnPacket packet);

    public Task ReceivePacket(IPacket packet);

    // Corresponding 1:1 to the actual packets
    // Shared
    public Task Ping();
    public Task Acknowledged();
    public Task Accepted(byte requestId);
    public Task Denied(byte requestId);
    public Task InvalidRequest(byte requestId, string errorMessage);
    // Server to client
    public Task LobbyCreated(byte lobbyId);
    public Task LobbyInfo(string lobbyInfo);
    public Task PlayerJoinedLobby(byte playerId, string profile);
    public Task PlayerLeftLobby(string displayName);
    public Task PlayerKicked();
    // public Task AvailableLobbies(string() info);
    public Task AvailableLobbies(string lobbyInfos);
    public Task GameStarting(byte lobbyId, DateTime startTime);
    // public Task GameSettingsChanged();
    // public Task PlayerProfileChanged();
    public Task RoleChangeRequested(byte playerId, string requestedRole);
    public Task RoleChanged(byte playerId, string newRole);
    public Task ExecuteTurn(string turnInfo);
    public Task BattleIsOver(); // TODO: send the winner?
    public Task UserMessage(string senderName, string content);
    public Task SystemMessage(string content);
    public Task ToggleReadyToStart(byte lobbyId, bool newStatus);
}