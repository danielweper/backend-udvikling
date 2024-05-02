using Core.Model;

namespace Core.Packets.Server;

public class PlayerJoinedLobbyPacket(byte playerId, IPlayerProfile profile) : IPacket
{
    public PacketType Type => PacketType.PlayerJoinedLobby;
    public readonly byte PlayerId = playerId;
    public readonly IPlayerProfile Profile = profile;
}