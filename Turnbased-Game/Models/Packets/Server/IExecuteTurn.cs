namespace Turnbased_Game.Models.Packets.Server;

public interface IExecuteTurn: IServerPackage
{
    public string TurnInfo { get; set; }
}