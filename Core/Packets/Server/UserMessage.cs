namespace Core.Packets.Server;

public class UserMessage : IPacket
{
    public PacketType type => PacketType.UserMessage;
}