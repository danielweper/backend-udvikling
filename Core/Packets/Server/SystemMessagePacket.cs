namespace Core.Packets.Server;

public class SystemMessagePacket(byte targetId, string content) : IPacket
{
    public PacketType Type => PacketType.SystemMessage;
    public readonly byte TargetId = targetId;
    public readonly string Content = content;
}