namespace Core.Packets.Shared;

public class Denied : IPacket
{
    public PacketType type => PacketType.Denied;
}