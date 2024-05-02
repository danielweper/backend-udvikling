namespace Core.Packets.Server;

public class LobbyCreatedPacket(byte lobbyId) : IPacket
{
    public PacketType Type => PacketType.LobbyCreated;
    public readonly byte LobbyId = lobbyId;
}