namespace Turnbased_Game.Models.Packets.Transport;

public abstract class PacketTransport
{
    public event Action<IPackage>? PacketSent;
    public event Action<IPackage>? PacketReceived;

    public virtual async Task<IPackage?> SendPacket(IPackage package)
    {
        PacketSent?.Invoke(package);
        return await Task.FromResult<IPackage?>(null);
    }

    public virtual void ReceivePacket(IPackage package)
    {
        PacketReceived?.Invoke(package);
    }
}