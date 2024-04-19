using Turnbased_Game.Models.Packets.Server;
using IHost = Turnbased_Game.Models.Client.IHost;

namespace Turnbased_Game.Models.ServerClasses;

public class LobbyInfo(byte Id, int playerCount, int maxPlayerCount, IHost Host) : ILobbyInfo
{
    public byte Id { get;} = Id;
    public int playerCount { get;} = playerCount;
    public int maxPlayerCount { get;} = maxPlayerCount;
    public IHost Host { get; } = Host;
}