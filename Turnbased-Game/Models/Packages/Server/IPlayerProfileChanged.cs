namespace Turnbased_Game.Models.Packages.Server;

public interface IPlayerProfileChanged: IServerPackage
{
    public int PlayerId { get; }   
    public IPlayerProfile UpdatedProfile { get; set;}
}