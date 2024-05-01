namespace Turnbased_Game.Models.Packages.Server;

public interface IGameStarting: IServerPackage
{
    DateTime StartTime{ get; set;}
}