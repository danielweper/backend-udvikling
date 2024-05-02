namespace Core.Packets.Server;

public class LobbyCreatedPacket : IPacket
{
    public PacketType Type => PacketType.LobbyCreated;
}