
using Turnbased_Game.Models.Server;
using Xunit;

namespace Turnbased_Game.Test;

public class ServerTests
{

    [Fact]
    public async Task Test_AddLobby()
    {
        // Arrange
        var server = new Server();
        var host = new Player("1",0);
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Private);
        // Act
        server.AddLobby(lobby);
        
        // Assert
        Assert.Contains(server.GetAvailableLobbies(), l => l.id == lobby.Id);
        Assert.False(server.LobbyIdIsFree(lobby.Id));
    }

    [Fact]
    public async Task Test_RemoveLobby()
    {
        // Arrange
        var server = new Server();
        var host = new Player("1",0);
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Private);
        server.AddLobby(lobby);
        
        Assert.Contains(server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        
        // Act
        server.RemoveLobby(lobby);
        
        // Assert
        Assert.DoesNotContain(server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.True(server.LobbyIdIsFree(lobby.Id));
    }
}