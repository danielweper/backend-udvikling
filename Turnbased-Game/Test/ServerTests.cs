using Moq;
using Turnbased_Game.Hubs;
using Core.Model;
using ServerLogic;
using Core.Model;
using Xunit;

namespace Turnbased_Game.Test;

public class ServerTests
{
    private readonly GameHub _hub;
    private Mock<IClient> _mockClient;

    public ServerTests()
    {
        _mockClient = new Mock<IClient>();
        _hub = new GameHub();
    }
    
    [Fact]
    public void Test_RemoveAndAddLobby()
    {
        // Arrange
        var server = new Server();
        
        var playerProfile = new PlayerProfile(Color.Red, "Cossai", "1");
        var host = new Player("Cossai", 0, playerProfile);
        
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

    [Fact]
    public void Test_GetAvailableLobbies()
    {
        // Arrange
        var server = new Server();
        
        var playerProfile = new PlayerProfile(Color.Red, "Cossai", "1");
        var host = new Player("Cossai",0,playerProfile);
        
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Private);
        
        // Act
        server.AddLobby(lobby);
        
        //Assert
        Assert.DoesNotContain(server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.False(server.LobbyIdIsFree(lobby.Id));
    }
}