namespace Core.Packets.Server;

public class GameStartingPacket : IPacket
{
    public PacketType type => PacketType.GameStarting;
}