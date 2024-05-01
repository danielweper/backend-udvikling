namespace Core.Packets.Shared;

public class InvalidRequest : IPacket
{
    public PacketType type => PacketType.InvalidRequest;
}