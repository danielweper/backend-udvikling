
using Turnbased_Game.Models;
using Turnbased_Game.Models.Server;
using static Turnbased_Game.Models.Color;
using Color = Turnbased_Game.Models.Color;
using Xunit;

namespace Turnbased_Game.Test;

public class ServerTests
{
    
    [Fact]
    public void Test_RemoveAndAddLobby()
    {
        // Arrange
        var server = new Server();
        
        var playerProfile = new PlayerProfile(Red, "Cossai", "1");
        var host = new Player("Cossai",0,playerProfile);
        
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Public);
        
        //Test_AddLobby
        server.AddLobby(lobby);
        
        //Check if lobby is added 
        Assert.Contains(server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.False(server.LobbyIdIsFree(lobby.Id));
        //Check if host is added as lobby host
        Assert.True(lobby.Host == host);
        
        // Act
        server.RemoveLobby(lobby);
        
        // Assert
        Assert.DoesNotContain(server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.True(server.LobbyIdIsFree(lobby.Id));
    }
}