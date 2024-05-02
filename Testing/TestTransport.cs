using Core.Packets.Transport;
using Core.Packets;
namespace Testing;

internal class TestTransport : PacketTransport
{
    public TestTransport()
    {
        this.PacketReceived += delegate (IPacket p) { lastReceived = p; };
        this.PacketSent += delegate (IPacket p) { lastSent = p; };
    }

    public IPacket? lastSent { get; protected set; }
    public IPacket? lastReceived { get; protected set; }

    public bool hasSent => (lastSent != null);
    public bool hasReceived => (lastReceived != null);
}
