namespace Core.Packets.Client;

public class CreateLobby : IPacket
{
    public PacketType type => PacketType.CreateLobby;
}