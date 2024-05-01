namespace Core.Packets.Shared;

public class Ping : IPacket
{
    public PacketType type => PacketType.Ping;
}