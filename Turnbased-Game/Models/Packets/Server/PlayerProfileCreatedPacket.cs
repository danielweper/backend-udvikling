using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public class PlayerProfileCreatedPacket(PlayerProfile playerProfile) : IPackage
{
    public PlayerProfile playerProfile { get; } = playerProfile;
}