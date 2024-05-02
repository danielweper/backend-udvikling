namespace Core.Packets.Shared;

public class InvalidRequestPacket : IPacket
{
    public PacketType Type => PacketType.InvalidRequest;
}