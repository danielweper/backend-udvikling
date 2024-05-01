namespace Core.Packets.Server;

public class LobbyCreatedPacket : IPacket
{
    public PacketType type => PacketType.LobbyCreated;
}