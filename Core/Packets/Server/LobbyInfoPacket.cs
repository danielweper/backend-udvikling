namespace Core.Packets.Server;

public class LobbyInfoPacket : IPacket
{
    public PacketType type => PacketType.LobbyInfo;
}