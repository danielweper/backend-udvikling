namespace Core.Packets.Server;

public class SystemMessagePacket(string content) : IPacket
{
    public PacketType Type => PacketType.SystemMessage;
    public readonly string Content = content;
}