using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public class RoleChangeRequestPacket(byte playerId, PlayerRole requestedRole) : IPackage
{
    public byte PlayerId { get; } = playerId;
    public PlayerRole RequestedRole { get; } = requestedRole;
}