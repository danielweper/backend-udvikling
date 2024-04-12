using Turnbased_Game.Models.Packages;

namespace Turnbased_Game.Models.Client;

public interface IClient : IChatter, IFighter, IHost
{
    byte id { get; }
    byte LastPackageId { get; }
    // some state for lobby
    // some state for players in lobby
    byte LobbyId { get; }

    public void SendPackage(IPackage package);
}