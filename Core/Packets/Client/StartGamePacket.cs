namespace Core.Packets.Client;

public class StartGamePacket : IPacket
{
    public PacketType type => PacketType.StartGame;
}