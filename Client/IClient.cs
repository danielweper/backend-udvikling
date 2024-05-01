using Core.Packets;

namespace ClientLogic;

public interface IClient : /*IChatter,*/ IFighter, IHost
{
    byte id { get; }
    // some state for lobby
    // some state for players in lobby
    byte lobbyId { get; }
    ClientStates currentState { get; }

    event Action? OnConnected;

    public void SendPackage(IPacket packet);
    public void ReceivePackage(IPacket packet);
}