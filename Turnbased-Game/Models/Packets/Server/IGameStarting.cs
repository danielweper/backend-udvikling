namespace Turnbased_Game.Models.Packets.Server;

public interface IGameStarting: IServerPackage
{
    DateTime StartTime{ get; set;}
}