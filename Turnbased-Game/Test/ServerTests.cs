
using Turnbased_Game.Models.Server;
using Xunit;

namespace Turnbased_Game.Test;

public class ServerTests
{
    
    [Fact]
    public async Task Test_RemoveLobby()
    {
        // Arrange
        var server = new Server();
        var host = new Player("Cossai",0);
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Private);
        //Test_AddLobby
        server.AddLobby(lobby);
        
        Assert.Contains(server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.False(server.LobbyIdIsFree(lobby.Id));
        
        // Act
        server.RemoveLobby(lobby);
        
        // Assert
        Assert.DoesNotContain(server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.True(server.LobbyIdIsFree(lobby.Id));
    }
}