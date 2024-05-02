namespace Core.Packets.Shared;

public class AcceptedPacket : IPacket
{
    public PacketType Type => PacketType.Accepted;
}