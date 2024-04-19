namespace Turnbased_Game.Models.Packets.Client;

public class RequestRoleChange : IPackage
{
    public byte id => 30;

    public PlayerRole newRole;
}