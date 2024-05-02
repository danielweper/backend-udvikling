namespace Core.Packets.Server;

public class UserMessagePacket(byte senderId, string content) : IPacket
{
    public PacketType Type => PacketType.UserMessage;
    public readonly byte SenderId = senderId;
    public readonly string Content = content;
}