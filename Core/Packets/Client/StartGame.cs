namespace Core.Packets.Client;

public class StartGame : IPacket
{
    public PacketType type => PacketType.StartGame;
}