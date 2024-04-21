using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Server;

namespace Turnbased_Game.Models.Server;

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
        List<LobbyInfo> lobbyInfo = new List<LobbyInfo>();
        foreach (Lobby lobby in _lobbies)
        {
            if (lobby.PlayerCount == lobby.MaxPlayerCount)
            {
                continue;
            }
            lobbyInfo.Add(lobby.GetInfo());
        }
        return lobbyInfo;
    }
}