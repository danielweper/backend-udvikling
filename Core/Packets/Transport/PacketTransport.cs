namespace Core.Packets.Transport;

public abstract class PacketTransport
{
    public bool IsConnected { get; protected set; } = false;

    public PacketTransport()
    {
        OnConnected += () => { IsConnected = true; };
        OnDisconnected += () => { IsConnected = false; };
    }

    public event Action<IPacket>? PacketSent;
    public event Action<IPacket>? PacketReceived;
    public event Action? OnConnected;
    public event Action? OnDisconnected;

    public virtual async Task<IPacket?> SendPacket(IPacket package)
    {
        PacketSent?.Invoke(package);
        return await Task.FromResult<IPacket?>(null);
    }

    public virtual void ReceivePacket(IPacket package)
    {
        PacketReceived?.Invoke(package);
    }

    protected void Connected() => OnConnected?.Invoke();
    protected void Disconnected() => OnDisconnected?.Invoke();
}