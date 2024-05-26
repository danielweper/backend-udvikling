namespace ServerLogic;

//TODO - Add GameInfo back
public readonly struct LobbyInfo(
    byte id,
    Player host,
    Player[] players,
    int maxPlayerCount,
    LobbyVisibility lobbyVisibility,
    GameInfo? gameInfo)
    // string gameInfo)
{
    public byte id { get; } = id;
    public Player host { get; } = host;
    public Player[] players { get; } = players.ToArray();
    public int maxPlayer { get; } = maxPlayerCount;

    public LobbyVisibility LobbyVisibility { get; } = lobbyVisibility;

    //public GameInfo? gameInfo { get; } = gameInfo;
    public GameInfo? gameInfo { get; } = gameInfo;

    public override string ToString()
    {
        return (
            $"{id} \n" +
            $"Host: {host.DisplayName} \n" +
            $"Players: {string.Join(", ", players.Select(player => player.DisplayName).ToArray())} \n" +
            $"Max players: {maxPlayer} \n" +
            $"Lobby visibility: {LobbyVisibility} \n" +
            $"{gameInfo} \n"
        );
    }
}