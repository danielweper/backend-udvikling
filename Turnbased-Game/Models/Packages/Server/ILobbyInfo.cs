using Turnbased_Game.Models.ServerClasses;
using IHost = Turnbased_Game.Models.Client.IHost;

namespace Turnbased_Game.Models.Packages.Server;

public interface ILobbyInfo: IServerPackage
{
    public byte Id { get; }
    public int playerCount { get; }
    public int maxPlayerCount {get;}
    public IHost Host { get; }
}