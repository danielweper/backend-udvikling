namespace Core.Packets.Shared;

public class AcceptedPacket(byte requestId) : IPacket
{
    public PacketType Type => PacketType.Accepted;
    public readonly byte RequestId = requestId;
}