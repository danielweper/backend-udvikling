namespace ServerLogic;

public readonly struct LobbyInfo(
    byte id,
    Player host,
    Player[] players,
    int maxPlayerCount,
    LobbyVisibility lobbyVisibility,
    GameInfo? gameInfo)
{
    public byte id { get; } = id;
    public Player host { get; } = host;
    public Player[] players { get; } = players.ToArray();
    public int maxPlayer { get; } = maxPlayerCount;
    public LobbyVisibility LobbyVisibility { get; } = lobbyVisibility;
    public GameInfo? gameInfo { get; } = gameInfo;

    public override string ToString()
    {
        return (
            $"LobbyId: {id} \n" +
            $"Host: {host} \n" +
            $"Players: {string.Join(", \n", players.Select(player => player.ToString()).ToArray())} \n" +
            $"MaxPlayers: {maxPlayer} \n" +
            $"Visibility: {LobbyVisibility} \n" +
            $"Game: {gameInfo?.ToString()} \n"
            );
    }
}