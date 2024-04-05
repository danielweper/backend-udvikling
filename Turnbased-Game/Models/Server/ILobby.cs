namespace Turnbased_Game.Models.Server;

public interface ILobby
{
    public void LobbyCreated();
    public void LobbyInfo(int lobbyId);
 
}