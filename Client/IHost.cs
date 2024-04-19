namespace Turnbased_Game.Models.Client;

public interface IHost : IParticipant
{
    public event Action<byte, IRole> RoleChangeRequested;

    public void CreateLobby();
    public void ChangeGameSettings(string settings); // JSON
    public void KickPlayer(byte playerId, string reason);
    public void CreateGame(string gameName);
    public void DeleteGame(string gameName);
    public void StartGame();
    public void Accepted(int requestId);
    public void Denied(int requestId);
}