using Moq;
using Turnbased_Game.Hubs;
using Core.Model;
using ServerLogic;

namespace Turnbased_Game.Test;

public class ServerTests
{
    private readonly GameHub _hub;
    private Mock<IHubClient> _mockClient;

    public ServerTests()
    {
        _mockClient = new Mock<IHubClient>();
        _hub = new GameHub();
    }
    
    [Fact]
    public void Test_RemoveAndAddLobby()
    {
        // Arrange
        // var Server = new Server();
        
        var playerProfile = new PlayerProfile(Color.Red, "Cossai");
        var host = new Player("Cossai", 0, playerProfile);
        
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Public);
        
        //Test_AddLobby
        Server.AddLobby(lobby);
        
        //Check if lobby is added 
        Assert.Contains(Server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.False(Server.LobbyIdIsFree(lobby.Id));
        //Check if host is added as lobby host
        Assert.True(lobby.Host == host);
        
        // Act
        Server.RemoveLobby(lobby);
        
        // Assert
        Assert.DoesNotContain(Server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.True(Server.LobbyIdIsFree(lobby.Id));
    }

    [Fact]
    public void Test_GetAvailableLobbies()
    {
        // Arrange
        // var Server = new Server();
        
        var playerProfile = new PlayerProfile(Color.Red, "Cossai");
        var host = new Player("Cossai",0,playerProfile);
        
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Private);
        
        // Act
        Server.AddLobby(lobby);
        
        //Assert
        Assert.DoesNotContain(Server.GetAvailableLobbies(), lobbyInfo => lobbyInfo.id == lobby.Id);
        Assert.False(Server.LobbyIdIsFree(lobby.Id));
    }

    /*[Fact]
    public void Test_AddPlayer()
    {
        //Arrange
        var server = new Server();
        
        var playerProfile = new PlayerProfile(Color.Red, "Cossai", "1");
        var host = new Player("Cossai",0,playerProfile);
        
        var felixProfile = new PlayerProfile(Color.Black, "Felix", "2");
        var player = new Player("Felix", 2, felixProfile);
        
        var lobby = new Lobby( 1, host, 9, LobbyVisibility.Private);
        
        // Act
        server.AddLobby(lobby);
        var lobbyInfo = server.GetLobbyInfo(1).GetValueOrDefault();
        
        Assert.Contains(lobbyInfo.host, lobbyPlayer => lobbyPlayer == player);

        server.GetLobby(1)?.AddPlayer(player);
        
        //Assert
    }*/
}