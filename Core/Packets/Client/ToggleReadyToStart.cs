namespace Core.Packets.Client;

public class ToggleReadyToStart : IPacket
{
    public PacketType type => PacketType.ToggleReadyToStart;

    public bool newStatus;
}