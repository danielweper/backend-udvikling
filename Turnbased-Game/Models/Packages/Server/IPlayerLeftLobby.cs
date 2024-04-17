namespace Turnbased_Game.Models.Packages.Server;

public interface IPlayerLeftLobby: IServerPackage
{
    public int PlayerId { get; }
}