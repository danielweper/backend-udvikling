using Core.Model;

namespace Core.Packets.Server;

public class RoleChangeRequestedPacket(byte playerId, IRole requestedRole) : IPacket
{
    public PacketType Type => PacketType.RoleChangeRequested;
    public readonly byte PlayerId = playerId;
    public readonly IRole RequestedRole = requestedRole;
}