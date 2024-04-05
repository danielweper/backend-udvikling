using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Client;

public interface IPlayer
{
    public void Ping();
    public void Acknowledged();
    public void CreateLobby();
    public void ListAvailableLobbies();
    public void JoinLobby(int lobbyId);
    public void DisconnectLobby();
    public void SubmitTurn(string turnInfo);
    public void SendMessage(IUserMessage userMessage);
    public void RequestRoleChange(IRole role);
    public void RequestProfileUpdate(IPlayerProfile profile);
    public void IsReady();
    public void IsNotReady();
}