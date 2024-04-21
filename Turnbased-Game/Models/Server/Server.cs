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
            if (_lobbies[i].id == requestId)
            {
                return _lobbies[i];
            }
        }

        return null;
    }
    public LobbyInfo? GetLobbyInfo(byte lobbyId) => GetLobby(lobbyId)?.GetInfo();

    public bool LobbyIdIsFree(byte lobbyId)
    {
        return _lobbies.All(lobby => lobby.id != lobbyId);
    }
    
    public List<string> GetAvailableLobbies()
    {
        List<string> lobbiesInfo = new List<string>();
        foreach (Lobby lobby in _lobbies)
        {
            LobbyInfo lobbyInfo = lobby.GetInfo();
            string info =
                $"Lobby id: {lobbyInfo}, Players: {lobbyInfo.players.Length}, MaxPlayers: {lobbyInfo.maxPlayer}";
            lobbiesInfo.Add(info);
        }
        return lobbiesInfo;
    }
}