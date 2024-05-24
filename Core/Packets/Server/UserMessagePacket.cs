namespace Core.Packets.Server;

public class UserMessagePacket(string senderName, string content) : IPacket
{
    public PacketType Type => PacketType.UserMessage;
    public readonly string SenderName = senderName;
    public readonly string Content = content;
}