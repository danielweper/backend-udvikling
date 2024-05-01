namespace Core.Packets.Client;

public class CreateLobbyPacket : IPacket
{
    public PacketType type => PacketType.CreateLobby;
}