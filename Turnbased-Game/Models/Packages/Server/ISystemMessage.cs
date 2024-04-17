namespace Turnbased_Game.Models.Packages.Server;

public interface ISystemMessage: IServerPackage
{
    public int TargetId { get; set; }
    public string Content { get; set; }
}