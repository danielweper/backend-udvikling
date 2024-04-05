namespace Turnbased_Game.Models.Client;

public interface IPlayer
{
    public void CreateLobby();
    public void ListAvailableLobbies();
    public void JoinLobby(int lobbyId);
    public void DisconnectLobby();
    public void SubmitTurn(string turn);
    public void SendMessage(int playerId, string message);
    public void RequestRoleChange(IRole role);
    public void RequestProfileUpdate(IPlayerProfile profile);
    public void IsReady();
    public void IsNotReady();
}