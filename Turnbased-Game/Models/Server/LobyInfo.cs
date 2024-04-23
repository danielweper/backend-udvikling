using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Server;

namespace Turnbased_Game.Models.Server;

public struct LobbyInfo(byte id, Player host, Player[] players, int maxPlayerCount, LobbyVisibility lobbyVisibility) : ILobbyInfo
{
    public LobbyInfo(byte id, Player host, List<Player> playerList, int maxPlayerCount, LobbyVisibility lobbyVisibility) :
        this(id, host, playerList.ToArray(), maxPlayerCount, lobbyVisibility) { }

    public byte id { get; } = id;
    public Player host { get; } = host;
    public Player[] players { get; } = players;
    public int maxPlayer { get; } = maxPlayerCount;
    public LobbyVisibility LobbyVisibility { get; } = lobbyVisibility;
}