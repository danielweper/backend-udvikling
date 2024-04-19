using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.ServerClasses;
using Host = Turnbased_Game.Models.Client.Host;
using IHost = Turnbased_Game.Models.Client.IHost;


namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IClient>
{
    private Server Server = new();
    private Random Random = new Random();

    public async Task CreateLobby(int maxPlayerCount)
    {
        await SendMessage("Received CreateLobby request"); // Acknowledged
        byte lobbyId = GenerateLobbyId();

        // Create host
        IClient caller = Clients.Caller;
        IHost host = new Host(caller.id);

        // Create lobby
        Lobby lobby = new Lobby(Host: host, id: lobbyId);
        Server.AddLobby(lobby);

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        CreateLobbyPacket packet = new CreateLobbyPacket(lobbyId);
        await Clients.Caller.CreateLobbyRequest(packet);
        
        
        LobbyInfo lobbyInfo = new LobbyInfo(maxPlayerCount: maxPlayerCount, Id: lobbyId, Host: host, playerCount: 1);
        await Clients.Caller.JoinLobbyRequest(new LobbyInfoPacket(lobbyInfo));
    }

    public async Task JoinLobby(IParticipant client, byte lobbyId)
    {
        Lobby? lobby = Server.GetLobby(lobbyId);


        if (lobby == null)
        {
            // No lobby found with corresponding id

            return await Clients.Caller.Denied();
        }

        IClient caller = Clients.Caller;

        IParticipant participant = new Participant(caller.id);

        Server.AddPlayerToLobby(participant, lobbyId);

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        await SendMessageToGroup($"{Context.ConnectionId} has joined the lobby with {lobbyId}", lobbyId);
    }


    private byte GenerateLobbyId()
    {
        byte lobbyId = (byte)Random.Next(1, 256);
        return lobbyId;
    }


    /*private async Task SendAcknowledged(string message)
    {
        ReceiveMessagePacket receiveMessagePacket = new ReceiveMessagePacket(content: message, dateTime: DateTime.Now);

        Acknowledged ackPack = new(message, DateTime.Now);

        await Clients.All.ReceiveAcknowledgePacket(ackPack);
    }*/

    private async Task SendMessageToGroup(string message, byte lobbyId)
    {
        ReceiveMessagePacket receiveMessagePacket = new ReceiveMessagePacket(content: message, dateTime: DateTime.Now);

        await Clients.Group($"{lobbyId}").ReceiveAcceptedPacket(receiveMessagePacket);
    }

    private async Task SendMessage(string message)
    {
        Acknowledged ackPack = new(message, DateTime.Now);

        await Clients.All.ReceiveAcknowledgePacket(ackPack);
    }

    public Task ReceiveMessage(string user, string message)
    {
        throw new NotImplementedException();
    }


    public Task Accepted(IAccepted content)
    {
        throw new NotImplementedException();
    }

    public Task Denied(IDenied content)
    {
        throw new NotImplementedException();
    }

    public Task InvalidRequest(IInvalidRequest content)
    {
        throw new NotImplementedException();
    }

    private void SendMessage(string message, MessageType type, IClient caller)
    {
        switch (type)
        {
            case MessageType.Accepted:
                Acknowledged ackPack = new(message, DateTime.Now);


                caller.Accepted();

                Caller.Denied(3);
        }
    }


    private SendMessageToGroup()
    {
    }

    enum MessageType
    {
        Accepted,
        Denied,
        Acknowledged
    }
}