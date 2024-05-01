namespace Core.Packets;

public abstract class PacketTransport
{
    public event Action<IPacket>? PacketSent;
    public event Action<IPacket>? PacketReceived;

    public virtual async Task<IPacket?> SendPacket(IPacket package)
    {
        PacketSent?.Invoke(package);
        return await Task.FromResult<IPacket?>(null);
    }

    public virtual void ReceivePacket(IPacket package)
    {
        PacketReceived?.Invoke(package);
    }
}