namespace Turnbased_Game.Models.Client;

public interface IParticipant
{
    public void CreateLobby();
    public void JoinLobby(int lobbyId);
    public void ListAvailableLobbies();
    public void DisconnectLobby();
    public void KickPlayer(int playerId, string reason);
    public void StartGame();
    
}