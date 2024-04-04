namespace DefaultNamespace;

public interface Player
{
    public void SubmitTurn(string turn);
    
    public void SendMessage(int playerId, string message);
    public void RequestRoleChange(Role role);
    public void RequestProfileUpdate(PlayerProfile profile);
    public void IsReady();
    public void IsNotReady();
}