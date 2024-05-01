namespace Core.Packets.Server;

public class LobbyCreated : IPacket
{
    public PacketType type => PacketType.LobbyCreated;
}