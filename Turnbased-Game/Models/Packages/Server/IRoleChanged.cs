namespace Turnbased_Game.Models.Packages.Server;

public interface IRoleChanged: IServerPackage
{
    int PlayerId { get; }
    public IRole NewRole { get; set;}

}