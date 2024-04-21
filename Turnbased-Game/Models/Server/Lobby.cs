using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Server;

namespace Turnbased_Game.Models.Server;

public class Lobby
{
    public readonly byte Id;
    
    public int MaxPlayerCount { get; private set; }
    public int PlayerCount => _players.Count;
    public Player Host => _players[0];
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    private List<Player> _players;
    private Game game;
    public LobbyVisibility Visibility { get; private set; }
    public Lobby(byte id, Player host, int maxPlayerCount = 10, LobbyVisibility visibility = LobbyVisibility.Public)
    {
        Id = id;
        _players = new List<Player>();
        _players.Add(host);
        MaxPlayerCount = maxPlayerCount;
        Visibility = visibility;
        game = new();
    }
    
    public LobbyInfo GetInfo()
    {
        return new LobbyInfo(Id, Host, _players, MaxPlayerCount);
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
        //TODO
        Game newGame = new Game();
        
    }
    public void LeaveGame(Game game)
    {
        //TODO
    }

    private void UpdateHost()
    {
        
    }
}
public enum LobbyVisibility
{
    Public,
    Private
}