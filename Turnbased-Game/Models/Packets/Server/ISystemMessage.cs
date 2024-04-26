namespace Turnbased_Game.Models.Packets.Server;

public interface ISystemMessage: IServerPackage
{
    public int TargetId { get; set; }
    public string Content { get; set; }
}