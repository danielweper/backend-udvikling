namespace Core.Packets.Client;

public class StartGamePacket : IPacket
{
    public PacketType Type => PacketType.StartGame;
}