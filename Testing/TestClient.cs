using ClientLogic;
using Core.Packets.Transport;
using Core.Packets;

namespace Testing;

internal class TestClient : Client
{
    public TestClient(byte lobbyId, IPacket? lastPackage, PacketTransport transporter) : base(transporter)
    {
        this.lobbyId = lobbyId;
        this.lastPackage = lastPackage;
    }

    public TestClient(byte playerId, byte lobbyId, IPacket? lastPackage) : this(lobbyId, lastPackage, new TestTransport()) { }

    public TestClient(byte playerId, byte lobbyId, PacketTransport transporter) : this(lobbyId, null, transporter) { }

    public TestClient(byte playerId, byte lobbyId) : this(playerId, lobbyId, (IPacket?)null) { }

    public TestClient(PacketTransport transporter) : this(1, 1, transporter) { }

    public TestClient() : this(1, 1) { }
    
    // For no refactoring purposes
    public TestClient(byte playerId, byte lobbyId, IPacket? lastPackage, PacketTransport transporter) : this(lobbyId, lastPackage, transporter) { }

    public IPacket? GetLastPackage() => this.lastPackage;
}
