namespace Core.Packets.Server;

public class PlayerJoinedLobbyPacket : IPacket
{
    public PacketType Type => PacketType.PlayerJoinedLobby;
}