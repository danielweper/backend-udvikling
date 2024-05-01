using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Turnbased_Game.Models.Packets;
using Turnbased_Game.Models.Packets.Transport;

namespace ClientCLI
{
    internal class CliTransporter : PacketTransport
    {

        public CliTransporter() {
            this.PacketReceived += delegate (IPackage p) { lastReceived = p; };
            this.PacketSent += delegate (IPackage p) { lastSent = p; };
        }
        public IPackage? lastSent { get; protected set; }
        public IPackage? lastReceived { get; protected set; }

        public bool hasSent => (lastSent != null);
        public override async Task<IPackage?> SendPacket(IPackage package)
        {
            await base.SendPacket(package);
            return null;
        }

        public override void ReceivePacket(IPackage package)
        {
            
        }
    }
}