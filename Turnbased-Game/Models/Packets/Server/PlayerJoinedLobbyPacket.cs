using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public class PlayerJoinedLobbyPacket(int playerId, IPlayerProfile profile) : IPackage
{
    public int PlayerId { get; set; } = playerId;
    public IPlayerProfile Profile{ get; set; } = profile;
}