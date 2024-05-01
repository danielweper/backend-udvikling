namespace Core.Packets.Server;

public class ExecuteTurn : IPacket
{
    public PacketType type => PacketType.ExecuteTurn;
}