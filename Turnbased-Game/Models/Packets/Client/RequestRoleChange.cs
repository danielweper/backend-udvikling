namespace Turnbased_Game.Models.Packets.Client;

public class RequestRoleChange : IPackage
{
    public byte PacketId => 30;
    public IRole newRole;
}