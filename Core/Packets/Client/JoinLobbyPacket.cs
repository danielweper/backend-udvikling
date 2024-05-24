namespace Core.Packets.Client;

public class JoinLobbyPacket(byte lobbyId, string? name) : IPacket
{
    public PacketType Type => PacketType.JoinLobby;

    public readonly byte LobbyId = lobbyId;
    public readonly string? Name = name;
}