namespace Core.Packets.Shared;

public class InvalidRequestPacket(byte requestId, string errorMessage) : IPacket
{
    public PacketType Type => PacketType.InvalidRequest;
    public readonly byte RequestId = requestId;
    public readonly string ErrorMessage = errorMessage;
}