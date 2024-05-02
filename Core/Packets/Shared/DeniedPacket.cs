namespace Core.Packets.Shared;

public class DeniedPacket(byte requestId) : IPacket
{
    public PacketType Type => PacketType.Denied;
    public readonly byte RequestId = requestId;
}