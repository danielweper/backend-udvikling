namespace Core.Packets.Server;

public class ToggleReadyPacket(byte lobbyId, byte playerId, bool newStatus) : IPacket
{
    public PacketType Type => PacketType.ToggleReadyToStart;

    public byte LobbyId = lobbyId;
    public byte PlayerId = playerId;
    public readonly bool NewStatus = newStatus;
}