using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientLogic;
using Turnbased_Game.Models.Packets;
using Turnbased_Game.Models.Packets.Transport;

namespace Testing;

internal class TestClient : Client
{
    public TestClient(byte id, byte lobbyId, IPackage? lastPackage, PacketTransport transporter) : base(transporter)
    {
        this.id = id;
        this.lobbyId = lobbyId;
        this.lastPackage = lastPackage;
    }

    public TestClient(byte id, byte lobbyId, IPackage? lastPackage) : this(id, lobbyId, lastPackage, new TestTransport()) { }

    public TestClient(byte id, byte lobbyId, PacketTransport transporter) : this(id, lobbyId, null, transporter) { }

    public TestClient(byte id, byte lobbyId) : this(id, lobbyId, (IPackage?)null) { }

    public TestClient(PacketTransport transporter) : this(1, 1, transporter) { }

    public TestClient() : this(1, 1) { }

    public IPackage? GetLastPackage() => this.lastPackage;
}
