namespace ServerLogic;

public class Lobby : ILobby
{
    public byte Id { get; }
    public int MaxPlayerCount { get; private set; }
    public int PlayerCount => _players.Count;
    public bool IsFull => (PlayerCount >= MaxPlayerCount);
    public bool IsEmpty => (PlayerCount == 0);
    public Player Host => _players[0];
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    public LobbyVisibility Visibility { get; private set; }
    public Game? Game { get; private set; }
    private List<Player> _players;

    public Lobby(byte id, Player host, int maxPlayerCount = 10, LobbyVisibility visibility = LobbyVisibility.Public)
    {
        Id = id;
        _players = [host];
        MaxPlayerCount = maxPlayerCount;
        Visibility = visibility;
    }

    public LobbyInfo GetInfo()
    {
        return new LobbyInfo(Id, Host, _players.ToArray(), MaxPlayerCount, Visibility,
            "GameINFO"/*Game?.GetInfo()*/);
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
        player.ParticipantId = generateNextId();
    }

    public void RemovePlayer(Player player)
    {
        _players.Remove(player);
    }

    public void CreateNewGame(GameType gameType)
    {
        Game = new Game(gameType);
    }

    private byte nextId = 0;
    private byte generateNextId()
    {
        do
        {
            nextId++;
        } while (nextId != 0 && !_players.All((Player p) => p.ParticipantId == nextId));

        return nextId;
    }
}