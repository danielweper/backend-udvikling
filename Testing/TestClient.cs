using ClientLogic;
using Core.Packets.Transport;
using Core.Packets;

namespace Testing;

internal class TestClient : Client
{
    public TestClient(byte id, byte lobbyId, IPacket? lastPackage, PacketTransport transporter) : base(transporter)
    {
        this.id = id;
        this.lobbyId = lobbyId;
        this.lastPackage = lastPackage;
    }

    public TestClient(byte id, byte lobbyId, IPacket? lastPackage) : this(id, lobbyId, lastPackage, new TestTransport()) { }

    public TestClient(byte id, byte lobbyId, PacketTransport transporter) : this(id, lobbyId, null, transporter) { }

    public TestClient(byte id, byte lobbyId) : this(id, lobbyId, (IPacket?)null) { }

    public TestClient(PacketTransport transporter) : this(1, 1, transporter) { }

    public TestClient() : this(1, 1) { }

    public IPacket? GetLastPackage() => this.lastPackage;
}
