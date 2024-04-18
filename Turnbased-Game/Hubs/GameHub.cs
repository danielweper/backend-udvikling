using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packages.Client;
using Turnbased_Game.Models.Packages.Shared;
using Turnbased_Game.Models.ServerClasses;
using IHost = Turnbased_Game.Models.Client.IHost;
using ISystemMessage = Turnbased_Game.Models;
using IUserMessage = Turnbased_Game.Models;


namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IClient>
{
    private Server Server = new();

    public async Task CreateLobby()
    {
        await SendAcknowledged("Received CreateLobby request");

        byte lobbyId = GenerateLobbyId();
        CreateLobbyRequest response = new CreateLobbyRequest(lobbyId);


        if (true)
        {
            await Clients.Caller.ReceiveLobby(response);
        }
        else
        {
            await Clients.Caller.Denied(0);
        }
    }

    private byte GenerateLobbyId()
    {
        // Todo
        throw new NotImplementedException();
    }

    public async Task SendAcknowledged(string message)
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