using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packages.Client;
using Turnbased_Game.Models.Packages.Shared;
using Turnbased_Game.Models.ServerClasses;
using Host = Turnbased_Game.Models.Client.Host;
using IHost = Turnbased_Game.Models.Client.IHost;


namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IClient>
{
    private Server Server = new();
    private Random random = new Random();

    public async Task CreateLobby()
    {
        await SendMessage("Received CreateLobby request"); // Acknowledged

        byte lobbyId = GenerateLobbyId();

        // Create host
        var caller = Clients.Caller;

        IHost host = new Host(caller.id);

        // Create lobby
        Lobby lobby = new Lobby(host);

        Server.AddLobby(lobby);

        JoinLobbyRequest response = new JoinLobbyRequest(lobbyId);

        //IParticipant client = new Host();


        await JoinLobby(client: host, lobbyId);
    }

    public async Task JoinLobby(IParticipant client, byte lobbyId)
    {
        // Put IParticant in lobby
        // Put caller in group

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        await SendMessageToGroup($"{Context.ConnectionId} has joined the lobby with {lobbyId}", lobbyId);
    }


    private byte GenerateLobbyId()
    {
        byte lobbyId = (byte)random.Next(1, 256);
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

        Acknowledged ackPack = new(message, DateTime.Now);

        await Clients.Group($"{lobbyId}").ReceiveAcknowledgePacket(ackPack);
    }

    private async Task SendMessage(string message)
    {
        ReceiveMessagePacket receiveMessagePacket = new ReceiveMessagePacket(content: message, dateTime: DateTime.Now);

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
}