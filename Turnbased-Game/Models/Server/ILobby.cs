namespace Turnbased_Game.Models.Server;

public interface ILobby
{
    public void Ping();
    public void Acknowledge();
    public void Accepted();
    public void Denied();
    public void InvalidRequest(string errorMessage);
    public void LobbyCreated();
    public void LobbyInfo();
    public void PlayerJoined();
    public void PlayerLeft(int playerId);
    public void PlayerInfo(int playerId);
    public void ListAvailableLobbies();
    public void GameCreated();
    public void GameDeleted();
    public void GameStarting(int gameId);
    public void GameSettingsChanged(int gameId);
    public void PlayerProfileChanged(int playerId);
    public void RoleChangeRequest(int playerId, IRole requestedRole);
    public void RoleChanged();
    

}