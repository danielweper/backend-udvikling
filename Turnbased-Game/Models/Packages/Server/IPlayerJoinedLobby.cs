namespace Turnbased_Game.Models.Packages.Server;

public interface IPlayerJoinedLobby : IPackage
{
    public int PlayerId { get; set; }
    public IPlayerProfile Profile{ get; set; }
}