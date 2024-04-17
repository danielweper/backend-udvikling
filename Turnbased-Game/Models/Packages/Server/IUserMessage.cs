namespace Turnbased_Game.Models.Packages.Server;

public interface IUserMessage: IServerPackage
{
    public int SenderId { get; }
    public int TargetId { get; set; }
    public string Content { get; set; }
}