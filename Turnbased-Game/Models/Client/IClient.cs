using Turnbased_Game.Models.Packages;

namespace Turnbased_Game.Models.Client;

public interface IClient : IChatter, IFighter, IHost
{
    byte id { get; }
    IPackage lastPackage{ get; }
    // some state for lobby
    // some state for players in lobby
    byte lobbyId { get; }

    public void SendPackage(IPackage package);
    public void ReceivePackage(IPackage package);
}