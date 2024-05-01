using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public class PlayerRoleChangedPacket(byte playerId, PlayerRole requestedRole) : IPackage
{
    public byte PacketId => 29;
    public byte PlayerId { get; } = playerId;
    public PlayerRole RequestedRole { get; } = requestedRole;
}