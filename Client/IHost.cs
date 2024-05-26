using Core.Model;

namespace ClientLogic;

public interface IHost : IParticipant
{
    public event Action<byte, IRole> RoleChangeRequested;

    public void CreateLobby();
    public void ChangeGameSettings(string settings); // JSON
    public void KickPlayer(string kickPlayerName, string reason);
    public void StartGame();
    public void Accepted(int requestId);
    public void Denied(int requestId);
}