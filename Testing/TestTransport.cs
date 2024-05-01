using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnbased_Game.Models.Packets;
using Turnbased_Game.Models.Packets.Transport;

namespace Testing;

internal class TestTransport : PacketTransport
{
    public TestTransport()
    {
        this.PacketReceived += delegate (IPackage p) { lastReceived = p; };
        this.PacketSent += delegate (IPackage p) { lastSent = p; };
    }

    public IPackage? lastSent { get; protected set; }
    public IPackage? lastReceived { get; protected set; }

    public bool hasSent => (lastSent != null);
    public bool hasReceived => (lastReceived != null);
}
