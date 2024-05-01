namespace Turnbased_Game.Models.Server;

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
}