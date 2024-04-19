using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packages.Client;
using Turnbased_Game.Models.Packages.Shared;
using Turnbased_Game.Models.ServerClasses;
using Host = Turnbased_Game.Models.Client.Host;
using IHost = Turnbased_Game.Models.Client.IHost;
using ISystemMessage = Turnbased_Game.Models;
using IUserMessage = Turnbased_Game.Models;


namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IClient>
{
    private Server Server = new();

    public async Task CreateLobby()
    {
        byte lobbyId = GenerateLobbyId();

        // Create lobby

        Lobby lobby = new Lobby();


        Server.AddLobby(lobby);
        // Create host


        JoinLobbyRequest response = new JoinLobbyRequest(lobbyId);

        IParticipant client = new Host();

        await JoinLobby(client: client, lobbyId);
    }

    public async Task JoinLobby(IParticipant client, byte lobbyId)
    {
        // Put host in lobby
        // Put caller in group

        await Groups.AddToGroupAsync(Context.ConnectionId, $"{lobbyId}");
        await SendAcknowledgedToGroup($"{Context.ConnectionId} has joined the lobby with {lobbyId}", lobbyId);
    }


    private byte GenerateLobbyId()
    {
        // Todo
        throw new NotImplementedException();
    }


    /*private async Task SendAcknowledged(string message)
    {
        ReceiveMessagePacket receiveMessagePacket = new ReceiveMessagePacket(content: message, dateTime: DateTime.Now);

        Acknowledged ackPack = new(message, DateTime.Now);

        await Clients.All.ReceiveAcknowledgePacket(ackPack);
    }*/

    private async Task SendAcknowledgedToGroup(string message, byte lobbyId)
    {
        ReceiveMessagePacket receiveMessagePacket = new ReceiveMessagePacket(content: message, dateTime: DateTime.Now);

        Acknowledged ackPack = new(message, DateTime.Now);

        await Clients.Group($"{lobbyId}").ReceiveAcknowledgePacket(ackPack);
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