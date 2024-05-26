using Microsoft.AspNetCore.SignalR.Client;
using Core.Packets;
using Core.Packets.Transport;
using Core.Packets.Client;
using Core.Packets.Server;
using Core.Packets.Shared;
using ServerLogic;
using Core.Model;

namespace ClientCLI
{
    internal class CliTransporter : PacketTransport
    {
        private readonly HubConnection _connection;
        private readonly string _hubUrl = "http://localhost:5163/game";

        public CliTransporter() : base() {
            _connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl) 
                .Build();
            _connection.Closed += (Exception? e) => { Disconnected(); return Task.CompletedTask; };
            _connection.Reconnected += (string? s) => { Connected(); return Task.CompletedTask; };

            // Shared (and simple)
            _connection.On("Ping", () => ReceivePacket(new PingPacket()));
            _connection.On("Acknowledged", () => ReceivePacket(new AcknowledgedPacket()));
            _connection.On("Accepted", (byte requestId) => ReceivePacket(new AcceptedPacket(requestId)));
            _connection.On("Denied", (byte requestId) => ReceivePacket(new DeniedPacket(requestId)));
            _connection.On("InvalidRequest", (byte requestId, string errorMessage) => ReceivePacket(new InvalidRequestPacket(requestId, errorMessage)));
            
            // Not shared
            _connection.On("LobbyCreated", (byte lobbyId) => ReceivePacket(new LobbyCreatedPacket(lobbyId)));
            _connection.On("LobbyInfo", (string lobbyInfo) =>
            {
                ReceivePacket(new LobbyInfoPacket(lobbyInfo));
            }); 
            _connection.On("AvailableLobbies", (string lobbyInfos) => ReceivePacket(new AvailableLobbiesPacket(lobbyInfos)));

            
            //_connection.On("KickPlayer", (byte playerId,));
            _connection.On("UserMessage", (byte sender, string content) => ReceivePacket(new UserMessagePacket(sender, content)));
            _connection.On("PlayerJoinedLobby", (byte playerId, string playerInfo) => ReceivePacket(new PlayerJoinedLobbyPacket(playerId, new PlayerProfile(Color.Pink, "Name"))));


            StartConnectionAsync().Wait();

            if (_connection.State == HubConnectionState.Connected)
            {
                Connected();
            }
        }

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
        public override async Task<IPacket?> SendPacket(IPacket package)
        {
            switch (package.Type) {
                case PacketType.CreateLobby:
                    var createLobbyPacket = (CreateLobbyPacket)package;
                    Console.WriteLine($"Host name: {createLobbyPacket.HostName}");
                    PlayerProfile host = new(Color.Pink, createLobbyPacket.HostName);
                    await _connection.InvokeAsync($"{package.Type}", 10, 0, host);
                    break;
                case PacketType.JoinLobby:
                    var joinLobbyPacket = (JoinLobbyPacket)package;
                    PlayerProfile playerProfile = new(Color.Purple, joinLobbyPacket.PlayerName);
                    await _connection.InvokeAsync($"{package.Type}", joinLobbyPacket.LobbyId, playerProfile);
                    break;
                case PacketType.ListAvailableLobbies:
                    await _connection.InvokeAsync($"{package.Type}");
                    break;
                //case PacketType.ListAvailableLobbies:
                case PacketType.SendMessage:
                    var sendMessagePacket = (SendMessagePacket)package;
                    await _connection.InvokeAsync("SendMessage", sendMessagePacket.SenderId ,sendMessagePacket.Message);
                    break;
            }
            await base.SendPacket(package);
            return null;
        }

        public override async void ReceivePacket(IPacket package)
        {
            base.ReceivePacket(package);
        }
    }
}
