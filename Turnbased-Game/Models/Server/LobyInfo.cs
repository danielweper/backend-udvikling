using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Server;

namespace Turnbased_Game.Models.Server;

public struct LobbyInfo(byte id, Player host, Player[] players, int maxPlayerCount) : ILobbyInfo
{
    public LobbyInfo(byte id, Player host, List<Player> playerList, int maxPlayerCount) :
        this(id, host, playerList.ToArray(), maxPlayerCount) { }

    public byte id { get; } = id;
    public Player host { get; } = host;
    public Player[] players { get; } = players;
    public int maxPlayerCount { get; } = maxPlayerCount;
}