namespace Core.Packets.Client;

public class RequestRoleChange : IPacket
{
    public PacketType type => PacketType.RequestRoleChange;

    // public IRole newRole;
}