using IHost = Turnbased_Game.Models.Client.IHost;

namespace Turnbased_Game.Models.Packets.Server;

public interface ILobbyInfo: IServerPackage
{
    public byte Id { get; }
    public int playerCount { get; }
    public int maxPlayerCount {get;}
    public IHost Host { get; }
}