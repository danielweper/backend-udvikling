using Core.Model;

namespace Core.Packets.Client;

public class RequestRoleChangePacket(IRole newRole) : IPacket
{
    public PacketType Type => PacketType.RequestRoleChange;

    public readonly IRole NewRole = newRole;
}