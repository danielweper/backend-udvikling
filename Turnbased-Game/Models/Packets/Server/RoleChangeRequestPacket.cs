using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public class RoleChangeRequestPacket(byte playerId, PlayerRole requestedRole, IRole requestedRole1) : IPackage
{
    public byte PlayerId { get; } = playerId;
    public IRole RequestedRole { get; } = requestedRole1;
}