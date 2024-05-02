namespace ServerLogic;

public interface ILobby
{
    byte Id { get; }
    int MaxPlayerCount { get; }
    int PlayerCount { get; }
    Player Host { get; }
    IReadOnlyList<Player> Players { get; }
    LobbyVisibility Visibility { get; }
    Game? Game { get; }

    LobbyInfo GetInfo();
    void AddPlayer(Player player);
    void RemovePlayer(Player player);
}