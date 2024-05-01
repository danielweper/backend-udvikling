namespace Core.Packets.Server;

public class PlayerJoinedLobby : IPacket
{
    public PacketType type => PacketType.PlayerJoinedLobby;
}