using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public class PlayerJoinedLobbyPacket(int playerId, string displayName, PlayerProfile profile) : IPackage
{
    public byte PacketId => 15;
    public int PlayerId { get; } = playerId;
    public string DisplayName { get; } = displayName;
    public PlayerProfile Profile { get; } = profile;
}