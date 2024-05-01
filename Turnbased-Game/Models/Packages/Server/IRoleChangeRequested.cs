namespace Turnbased_Game.Models.Packages.Server;

public interface IRoleChangeRequested: IServerPackage
{
    public int PlayerId { get; set; }
    public IRole RequestedRole { get; set;}
}