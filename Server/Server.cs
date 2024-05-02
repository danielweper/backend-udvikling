namespace ServerLogic;

public static class Server
{
    
    public static List<Lobby> _lobbies = new List<Lobby>();
    public static IReadOnlyList<Lobby> Lobbies => _lobbies.AsReadOnly();

    /*public Server()
    {
        _lobbies = new();
    }*/

    public static void AddLobby(Lobby lobby)
    {
        _lobbies.Add(lobby);
    }
    public static void RemoveLobby(Lobby lobby)
    {
        _lobbies.Remove(lobby);
    }

    public static Lobby? GetLobby(byte requestId) => _lobbies.FirstOrDefault((Lobby l) => l.Id == requestId);

    public static LobbyInfo? GetLobbyInfo(byte lobbyId) => GetLobby(lobbyId)?.GetInfo();

    public static bool LobbyIdIsFree(byte lobbyId)
    {
        return _lobbies.All(lobby => lobby.Id != lobbyId);
    }

    public static List<LobbyInfo> GetAvailableLobbies()
    {
        return (from lobby in _lobbies where lobby.PlayerCount != lobby.MaxPlayerCount && lobby.Visibility == LobbyVisibility.Public select lobby.GetInfo()).ToList();
    }
}