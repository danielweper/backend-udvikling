using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Turnbased_Game.Models.Packets;
using Turnbased_Game.Models.Packets.Transport;

namespace ClientCLI
{
    internal class CliTransporter : PacketTransport
    {
        private readonly HubConnection _connection;
        private readonly string _hubUrl = "http://localhost:YOUR_PORT/gameHub";

        public CliTransporter() {
            this.PacketReceived += delegate (IPackage p) { lastReceived = p; };
            this.PacketSent += delegate (IPackage p) { lastSent = p; };
            _connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl) 
                .Build();
            _connection.On<IPackage>("SendPackage", package => Console.WriteLine($"Send package with package id: {package.id}"));
            StartConnectionAsync().Wait();
        }
        public IPackage? lastSent { get; protected set; }
        public IPackage? lastReceived { get; protected set; }

        public bool hasSent => (lastSent != null);
        public bool hasReceived => (lastReceived != null);
        private async Task StartConnectionAsync()
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("SignalR connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error establishing SignalR connection: {ex.Message}");
            }
        }
        public override async Task<IPackage?> SendPacket(IPackage package)
        {
            await base.SendPacket(package);
            await _connection.StartAsync();
            await _connection.InvokeAsync("ReceivePacket", package);
            return null;
        }

        public override void ReceivePacket(IPackage package)
        {
            
        }
    }
}
