using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Packages;
using Turnbased_Game.Models.Packages.Server;
using IServer = Turnbased_Game.Models.Server.IServer;


namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IServer>
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.ReceiveMessage(user, message);
    }

    public async Task SendAcknowledge(IAcknowledged acknowledgedPacket, string clientId)
    {
        await Clients.Client(clientId).Acknowledge(acknowledgedPacket);
    }
    
    
}