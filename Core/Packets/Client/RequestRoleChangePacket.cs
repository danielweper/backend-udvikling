namespace Core.Packets.Client;

public class RequestRoleChangePacket : IPacket
{
    public PacketType type => PacketType.RequestRoleChange;

    // public IRole newRole;
}