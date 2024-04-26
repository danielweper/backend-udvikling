namespace Turnbased_Game.Models.Packets.Server;

public interface IUserMessage: IServerPackage
{
    public int SenderId { get; }
    public int TargetId { get; set; }
    public string Content { get; set; }
}