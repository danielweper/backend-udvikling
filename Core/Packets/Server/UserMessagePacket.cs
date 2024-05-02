namespace Core.Packets.Server;

public class UserMessagePacket : IPacket
{
    public PacketType Type => PacketType.UserMessage;
}