namespace Core.Packets.Shared;

public class PingPacket : IPacket
{
    public PacketType Type => PacketType.Ping;
}