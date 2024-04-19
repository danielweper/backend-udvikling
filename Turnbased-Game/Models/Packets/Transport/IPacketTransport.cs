namespace Turnbased_Game.Models.Packets.Transport;

public interface IPacketTransport
{
    public event Action<IPackage> PacketSent;
    public event Action<IPackage> PacketReceived;

    public void SendPacket(IPackage package);
    public void ReceivePacket(IPackage package);
}