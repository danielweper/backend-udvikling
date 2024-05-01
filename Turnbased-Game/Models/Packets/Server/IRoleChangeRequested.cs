namespace Turnbased_Game.Models.Packets.Server;

public interface IRoleChangeRequested: IServerPackage
{
    public int PlayerId { get; set; }
    public IRole RequestedRole { get; set;}
}