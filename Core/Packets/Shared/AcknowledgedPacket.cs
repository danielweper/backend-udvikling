namespace Core.Packets.Shared;

public class AcknowledgedPacket : IPacket
{
    public PacketType type => PacketType.Acknowledged;
}