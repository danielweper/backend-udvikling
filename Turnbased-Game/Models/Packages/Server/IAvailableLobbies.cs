namespace Turnbased_Game.Models.Packages.Server;

public interface IAvailableLobbies: IServerPackage
{
    public String[] LobbyInfo { get; set;}

    //public ILobbyInfo getLobbies();
    
}