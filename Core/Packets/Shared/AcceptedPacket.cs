namespace Core.Packets.Shared;

public class AcceptedPacket : IPacket
{
    public PacketType type => PacketType.Accepted;
}