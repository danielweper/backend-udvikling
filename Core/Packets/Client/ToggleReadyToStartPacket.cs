namespace Core.Packets.Client;

public class ToggleReadyToStartPacket(bool newStatus) : IPacket
{
    public PacketType Type => PacketType.ToggleReadyToStart;

    public readonly bool NewStatus = newStatus;
}