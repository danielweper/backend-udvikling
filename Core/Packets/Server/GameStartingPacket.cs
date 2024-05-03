namespace Core.Packets.Server;

public class GameStartingPacket(DateTime time) : IPacket
{
    public PacketType Type => PacketType.GameStarting;
    public DateTime Startingtime = time;
}