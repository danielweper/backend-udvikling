using Core.Packets;
using Core.Packets.Transport;

namespace ClientLogic;

public interface IClient : /*IChatter,*/ IFighter, IHost
{
    // byte? playerId { get; }
    // some state for lobby
    // some state for players in lobby
    byte? lobbyId { get; }
    ClientStates CurrentState { get; }
    PacketTransport Transporter { get; }

    public void SendPackage(IPacket packet);
    public void ReceivePackage(IPacket packet);
}