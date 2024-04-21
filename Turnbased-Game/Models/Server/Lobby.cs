namespace Turnbased_Game.Models.Server;

public class Lobby
{
    public readonly byte Id;
    public int MaxPlayerCount { get; private set; }
    public int PlayerCount => _players.Count;
    public Player Host => _players[0];
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    private List<Player> _players;
    private Game _game;
    public LobbyVisibility Visibility { get; private set; }

    public Lobby(byte id, Player host, int maxPlayerCount = 10, LobbyVisibility visibility = LobbyVisibility.Public)
    {
        Id = id;
        _players = new List<Player>();
        _players.Add(host);
        MaxPlayerCount = maxPlayerCount;
        Visibility = visibility;
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

    public void CreateNewGame(GameType gameType)
    {
        _game = new Game(gameType);
    }

    public Game? GetGame()
    {
        return _game;
    }

    public void LeaveGame(Game game)
    {
        //TODO
    }

    public void UpdatePlayerId()
    {
        _players = _players.OrderByDescending(pl => pl.id).ToList();

        byte i = 0;
        foreach (var player in _players)
        {
            player.id = i;
            i++;
        }
    }
}

public enum LobbyVisibility
{
    Public,
    Private
}