namespace Turnbased_Game.Models.Client;

public interface IHost: IPlayer
{
    public void ChangeGameSettings(string settings); // JSON
    public void KickPlayer(int playerId, string reason);
    public void CreateGame();
    public void DeleteGame();
    public void StartGame();
    public void Accepted(int requestId);
    public void Denied(int requestId);
}