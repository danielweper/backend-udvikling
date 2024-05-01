namespace Core.Packets.Server;

public class ExecuteTurnPacket : IPacket
{
    public PacketType type => PacketType.ExecuteTurn;
}