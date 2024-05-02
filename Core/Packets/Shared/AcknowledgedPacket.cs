namespace Core.Packets.Shared;

public class AcknowledgedPacket : IPacket
{
    public PacketType Type => PacketType.Acknowledged;
}