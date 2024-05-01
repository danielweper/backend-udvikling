namespace Core.Packets.Client;

public class JoinLobbyPacket : IPacket
{
    public PacketType type => PacketType.JoinLobby;

    public byte lobbyId;
}