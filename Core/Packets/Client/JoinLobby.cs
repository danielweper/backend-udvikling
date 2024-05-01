namespace Core.Packets.Client;

public class JoinLobby : IPacket
{
    public PacketType type => PacketType.JoinLobby;

    public byte lobbyId;
}