namespace Core.Packets.Server;

public class RoleChangeRequested : IPacket
{
    public PacketType type => PacketType.RoleChangeRequested;
    public int PlayerId { get; set; }
    // public IRole RequestedRole { get; set;}
}