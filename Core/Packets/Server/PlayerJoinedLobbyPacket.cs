namespace Core.Packets.Server;

public class PlayerJoinedLobbyPacket : IPacket
{
    public PacketType type => PacketType.PlayerJoinedLobby;
}