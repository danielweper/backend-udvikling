using Core.Model;

namespace Core.Packets.Server;

public class RoleChangedPacket(byte playerId, IRole newRole) : IPacket
{
    public PacketType Type => PacketType.ChangeGameSettings;
    public readonly byte PlayerId = playerId;
    public readonly IRole NewRole = newRole;
}