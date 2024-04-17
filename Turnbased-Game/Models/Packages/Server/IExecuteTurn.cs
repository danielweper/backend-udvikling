namespace Turnbased_Game.Models.Packages.Server;

public interface IExecuteTurn: IServerPackage
{
    public string TurnInfo { get; set; }
}