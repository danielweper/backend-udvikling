namespace Core.Packets.Client;

public class SendMessagePacket(byte senderId, string message) : IPacket
{
    public PacketType Type => PacketType.SendMessage;
    public readonly byte SenderId = senderId;
    public readonly string Message = message;
}