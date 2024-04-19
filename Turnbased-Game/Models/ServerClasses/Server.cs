using Turnbased_Game.Models.Client;

namespace Turnbased_Game.Models.ServerClasses;

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
            if (_lobbies[i].Id == requestId)
            {
                return _lobbies[i];
            }
        }

        return null;
    }

    public void AddPlayerToLobby(IParticipant participant, byte lobbyId)
    {
        GetLobby(lobbyId).AddPlayerToLobby(participant);
    }
}