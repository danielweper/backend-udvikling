using Microsoft.AspNetCore.SignalR;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packages.Client;
using Turnbased_Game.Models.Packages.Server;


namespace Turnbased_Game.Hubs;

public class GameHub : Hub<IClient>
{
    public async Task CreateLobby(ICreateLobby request)
    {
        string lobbyId = GenerateLobbyId();

      
    }

    private string GenerateLobbyId()
    {
        // Todo
        throw new NotImplementedException();
    }
}