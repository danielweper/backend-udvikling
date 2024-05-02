namespace Core.Packets.Server;

public class PlayerLeftLobbyPacket(byte playerId) : IPacket
{
    public PacketType Type => PacketType.PlayerLeftLobby;
    public readonly byte PlayerId = playerId;
}