namespace Core.Packets.Client;

public class ToggleReadyToStartPacket(byte lobbyId, byte playerId, bool newStatus) : IPacket
{
    public PacketType Type => PacketType.ToggleReadyToStart;

    public readonly byte LobbyId = lobbyId;
    public readonly byte PlayerId = playerId;
    public readonly bool NewStatus = newStatus;
}