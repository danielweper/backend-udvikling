namespace Core.Packets.Shared;

public class Accepted : IPacket
{
    public PacketType type => PacketType.Accepted;
}