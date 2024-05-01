namespace ServerLogic;

public class Server
{
    private readonly List<Lobby> _lobbies;

    public Server()
    {
        _lobbies = new();
    }

    public void AddLobby(Lobby lobby)
    {
        _lobbies.Add(lobby);
    }
    public void RemoveLobby(Lobby lobby)
    {
        _lobbies.Remove(lobby);
    }

    public Lobby? GetLobby(byte requestId)
    {
        for (int i = 0; i < _lobbies.Count; i++)
        {
            if (_lobbies[i].Id == requestId)
            {
                return _lobbies[i];
            }
        }

        return null;
    }

    public LobbyInfo? GetLobbyInfo(byte lobbyId) => GetLobby(lobbyId)?.GetInfo();

    public bool LobbyIdIsFree(byte lobbyId)
    {
        return _lobbies.All(lobby => lobby.Id != lobbyId);
    }

    public List<LobbyInfo> GetAvailableLobbies()
    {
        return (from lobby in _lobbies where lobby.PlayerCount != lobby.MaxPlayerCount && lobby.Visibility == LobbyVisibility.Public select lobby.GetInfo()).ToList();
    }
}