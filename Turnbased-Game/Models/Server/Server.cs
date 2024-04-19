using Turnbased_Game.Models.Client;

namespace Turnbased_Game.Models.Server;

public class Server
{
    private List<Lobby> _lobbies;

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
            if (_lobbies[i].id == requestId)
            {
                return _lobbies[i];
            }
        }

        return null;
    }

    public void AddPlayerToLobby(Player participant, Lobby lobby)
    {
        lobby.AddPlayer(participant);
    }

    public LobbyInfo? GetLobbyInfo(byte lobbyId) => GetLobby(lobbyId)?.GetInfo();

    public bool LobbyIdIsFree(byte lobbyId)
    {
        foreach (Lobby lobby in _lobbies)
        {
            // todo
            
        }

        return true;
    }
}