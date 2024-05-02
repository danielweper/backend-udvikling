namespace Core.Packets.Server;

public class ExecuteTurnPacket : IPacket
{
    public PacketType Type => PacketType.ExecuteTurn;
}