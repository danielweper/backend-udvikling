namespace Turnbased_Game.Models.Packages.Server;

public interface ILobbyCreated: IServerPackage
{
    public int LobbyId { get; set; }
}