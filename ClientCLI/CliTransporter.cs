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

        public CliTransporter() : base()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();
            _connection.Closed += (Exception? e) =>
            {
                Disconnected();
                return Task.CompletedTask;
            };
            _connection.Reconnected += (string? s) =>
            {
                Connected();
                return Task.CompletedTask;
            };

            // Shared (and simple)
            _connection.On("Ping", () => ReceivePacket(new PingPacket()));
            _connection.On("Acknowledged", () => ReceivePacket(new AcknowledgedPacket()));
            _connection.On("Accepted", (byte requestId) => ReceivePacket(new AcceptedPacket(requestId)));
            _connection.On("Denied", (byte requestId) => ReceivePacket(new DeniedPacket(requestId)));
            _connection.On("InvalidRequest",
                (byte requestId, string errorMessage) =>
                    ReceivePacket(new InvalidRequestPacket(requestId, errorMessage)));

            // Not shared
            _connection.On("LobbyCreated", (byte lobbyId) => ReceivePacket(new LobbyCreatedPacket(lobbyId)));
            _connection.On("LobbyInfo", (string lobbyInfo) => { ReceivePacket(new LobbyInfoPacket(lobbyInfo)); });
            _connection.On("AvailableLobbies",
                (string lobbyInfos) => ReceivePacket(new AvailableLobbiesPacket(lobbyInfos)));


            //Message  
            _connection.On("UserMessage",
                (string sender, string content) => ReceivePacket(new UserMessagePacket(sender, content)));

            _connection.On("PlayerJoinedLobby",
                (byte playerId, string playerInfo) =>
                    ReceivePacket(new PlayerJoinedLobbyPacket(playerId, new PlayerProfile(Color.Pink, "Name"))));
            //_connection.On("KickPlayer", (byte playerId,));

            //Game
            _connection.On("ToggleReadyToStart",
                (byte lobbyId, byte playerId, bool status) =>
                    ReceivePacket(new ToggleReadyPacket(lobbyId, playerId, status)));
            _connection.On("GameStarting",
                (byte lobbyId, DateTime time) => ReceivePacket(new StartGamePacket(lobbyId, time)));
            //Battle

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
            switch (package.Type)
            {
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
                    await _connection.InvokeAsync($"{package.Type}", sendMessagePacket.Message);
                    break;
                case PacketType.StartGame:
                    var gameStartingPacket = (StartGamePacket)package;
                    await _connection.InvokeAsync("StartGame", gameStartingPacket.LobbyId);
                    Console.WriteLine($"Game created at: {gameStartingPacket.Time}");
                    break;
                case PacketType.ToggleReadyToStart:
                    var toggleReadyToStart = ((ToggleReadyToStartPacket)package);
                    await _connection.InvokeAsync("ToggleIsPlayerReady", toggleReadyToStart.LobbyId,
                        toggleReadyToStart.NewStatus);
                    if (toggleReadyToStart.NewStatus)
                    {
                        Console.WriteLine("I'm ready");
                    }
                    else
                    {
                        Console.WriteLine("I'm not ready");
                    }
                    break;
                case PacketType.DisconnectLobby:
                    var disconnectLobbyPacket = (DisconnectLobbyPacket)package;
                    await _connection.InvokeAsync("LeaveLobby", disconnectLobbyPacket.LobbyId);
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