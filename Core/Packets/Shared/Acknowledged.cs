namespace Core.Packets.Shared;

public class Acknowledged : IPacket
{
    public PacketType type => PacketType.Acknowledged;
}