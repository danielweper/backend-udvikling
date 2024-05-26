using ServerLogic;

namespace Turnbased_Game.Models;

public static class ConnectionKnower
{
    private static readonly HashSet<string> NonPlayerConnections = new();
    private static readonly HashSet<string> PlayerConnections = new();
    private static readonly Dictionary<string, Player> ConnectionToPlayer = new();
    private static readonly Dictionary<string, Lobby> ConnectionToLobby = new();

    public static void AddConnection(string connectionId)
    {
        NonPlayerConnections.Add(connectionId);
    }

    public static void RemoveConnection(string connectionId)
    {
        NonPlayerConnections.Remove(connectionId);
        PlayerConnections.Remove(connectionId);
        ConnectionToPlayer.Remove(connectionId);
        ConnectionToLobby.Remove(connectionId);
    }

    public static void MakePlayerConnection(string connectionId, Player player, Lobby lobby)
    {
        NonPlayerConnections.Remove(connectionId);
        PlayerConnections.Add(connectionId);
        ConnectionToPlayer[connectionId] = player;
        ConnectionToLobby[connectionId] = lobby;
    }

    public static void MakeNonPlayerConnection(string connectionId)
    {
        NonPlayerConnections.Add(connectionId);
        PlayerConnections.Remove(connectionId);
        ConnectionToPlayer.Remove(connectionId);
        ConnectionToLobby.Remove(connectionId);
    }

    public static bool IsPlayer(string connectionId) => PlayerConnections.Contains(connectionId);
    public static Player? GetPlayer(string connectionId) => (IsPlayer(connectionId) ? ConnectionToPlayer[connectionId] : null);
    public static Lobby? GetLobby(string connectionId) => (IsPlayer(connectionId) ? ConnectionToLobby[connectionId] : null);
    public static string GetConnection(Player player) => SearchForKey(player, ConnectionToPlayer);
    public static string GetConnection(Lobby lobby) => SearchForKey(lobby, ConnectionToLobby);

    private static string SearchForKey<T>(T searchingFor, Dictionary<string, T> dict) =>
        dict.Where(pair => pair.Value!.Equals(searchingFor)).First().Key;
}