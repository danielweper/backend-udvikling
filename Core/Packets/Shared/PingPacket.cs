namespace Core.Packets.Shared;

public class PingPacket : IPacket
{
    public PacketType type => PacketType.Ping;
}