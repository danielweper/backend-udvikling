using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Server;

namespace Turnbased_Game.Models.Server;

public class Lobby
{
    public readonly byte id;
    
    public int maxPlayerCount { get; private set; }
    public int playerCount => _players.Count;
    public Player host => _players[0];
    public IReadOnlyList<Player> players => _players.AsReadOnly();
    private List<Player> _players;
    private List<Game> _games;
    public LobbyVisibility visibility { get; private set; }
    public Lobby(byte id, Player host, int maxPlayerCount = 10, LobbyVisibility visibility = LobbyVisibility.Public)
    {
        this.id = id;
        _players = new List<Player>();
        _players.Add(host);
        this.maxPlayerCount = maxPlayerCount;
        this.visibility = visibility;
    }
    
    public LobbyInfo GetInfo()
    {
        return new LobbyInfo(id, host, _players, maxPlayerCount);
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }
    public void RemovePlayer(Player player)
    {
        _players.Remove(player);
    }
    public void CreateGame()
    {
        Game newGame = new Game();
        _games.Add(newGame);
        
    }
    public void LeaveGame(Game game)
    {
        
    }
}
public enum LobbyVisibility
{
    Public,
    Private
}