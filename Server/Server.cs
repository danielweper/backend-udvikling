namespace ServerLogic;

public class Server
{
    public List<Lobby> _lobbies;
    public IReadOnlyList<Lobby> Lobbies => _lobbies.AsReadOnly();

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

    public Lobby? GetLobby(byte requestId) => _lobbies.FirstOrDefault((Lobby l) => l.Id == requestId);

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