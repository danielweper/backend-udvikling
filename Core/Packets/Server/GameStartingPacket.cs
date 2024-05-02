namespace Core.Packets.Server;

public class GameStartingPacket : IPacket
{
    public PacketType Type => PacketType.GameStarting;
}