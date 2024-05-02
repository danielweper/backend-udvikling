namespace Core.Packets.Client;

public class JoinLobbyPacket(byte lobbyId) : IPacket
{
    public PacketType Type => PacketType.JoinLobby;

    public readonly byte LobbyId = lobbyId;
}