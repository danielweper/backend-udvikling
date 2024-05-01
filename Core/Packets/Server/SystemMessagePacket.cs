namespace Core.Packets.Server;

public class SystemMessagePacket : IPacket
{
    public PacketType type => PacketType.SystemMessage;
    public int TargetId { get; set; }
    public string Content { get; set; }
}