namespace Core.Packets.Server;

public class GameStarting : IPacket
{
    public PacketType type => PacketType.GameStarting;
}