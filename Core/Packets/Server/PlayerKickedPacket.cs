namespace Core.Packets.Server;

public class PlayerKickedPacket : IPacket
{
    public PacketType Type => PacketType.PlayerKicked;
}