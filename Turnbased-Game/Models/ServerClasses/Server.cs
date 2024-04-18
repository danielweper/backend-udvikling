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
}