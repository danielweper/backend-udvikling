namespace Core.Packets.Server;

public class LobbyInfo : IPacket
{
    public PacketType type => PacketType.LobbyInfo;
}