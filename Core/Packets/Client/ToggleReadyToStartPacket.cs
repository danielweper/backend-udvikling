namespace Core.Packets.Client;

public class ToggleReadyToStartPacket(byte lobbyId, bool newStatus) : IPacket
{
    public PacketType Type => PacketType.ToggleReadyToStart;

    public readonly byte LobbyId = lobbyId;
    public readonly bool NewStatus = newStatus;
}