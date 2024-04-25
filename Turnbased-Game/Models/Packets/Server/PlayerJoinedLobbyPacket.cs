using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public class PlayerJoinedLobbyPacket(int playerId, PlayerProfile profile) : IPackage
{
    public byte PacketId => 15;
    public int PlayerId { get; set; } = playerId;
    public PlayerProfile Profile{ get; set; } = profile;
}