namespace Core.Packets.Server;

public class UserMessagePacket : IPacket
{
    public PacketType type => PacketType.UserMessage;
}