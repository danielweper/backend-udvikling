namespace Core.Packets.Client;

public class DisconnectLobbyPacket(byte lobbyId) : IPacket
{
    public PacketType Type => PacketType.DisconnectLobby;
    public byte LobbyId { get; } = lobbyId;
}