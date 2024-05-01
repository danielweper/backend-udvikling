namespace Core.Packets.Shared;

public class InvalidRequestPacket : IPacket
{
    public PacketType type => PacketType.InvalidRequest;
}