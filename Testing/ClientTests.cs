using Moq;
using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Client;
using Turnbased_Game.Models.Packets;

namespace Testing;

public class ClientTests
{
    TestTransport testTransporter = new TestTransport();

    [Fact]
    public void Test_SendPackage_UpdatesTheLastPackageId()
    {
        // Arrange
        var client = new TestClient();
        var packageMock = new Mock<IPackage>();
        byte packageId = 11; // Metapod

        // Set up mock behavior
        packageMock.Setup(p => p.id).Returns(packageId);

        // Act
        client.SendPackage(packageMock.Object);

        // Assert
        Assert.Equal(packageMock.Object, client.GetLastPackage());
    }

    [Fact]
    public void Test_SubmitTurn_TurnInfoGetsThrough()
    {
        // Arrange
        var client = new Client(testTransporter);
        string payload = "turn: 1";

        // Act
        client.SubmitTurn(payload);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(32, testTransporter.lastSent.id);
        SubmitTurn sentPacket = (SubmitTurn)testTransporter.lastSent;
        Assert.Equal(payload, sentPacket.turnInfo);
    }

    [Fact]
    public void Test_ListAvailableLobbies_PacketGetsSent()
    {
        // Arrange
        var client = new Client(testTransporter);

        // Act
        client.ListAvailableLobbies();

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(16, testTransporter.lastSent.id);
    }

    [Fact]
    public void Test_JoinLobby_JoiningTheCorrectLobby()
    {
        // Arrange
        var client = new Client(testTransporter);
        byte lobbyId = 42;

        // Act
        client.JoinLobby(lobbyId);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(14, testTransporter.lastSent.id);
        var sentPacket = (JoinLobby)testTransporter.lastSent;
        Assert.Equal(lobbyId, sentPacket.lobbyId);
    }

    [Fact]
    public void Test_DisconnectLobby_PacketGetsSent()
    {
        // Arrange
        var client = new Client(testTransporter);

        // Act
        client.ListAvailableLobbies();

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(16, testTransporter.lastSent.id);
    }

    [Fact]
    public void Test_IsReady_PacketGetsSent()
    {
        // Arrange
        var client = new Client(testTransporter);

        // Act
        client.IsReady();

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(24, testTransporter.lastSent.id);
        var sentPacket = (ToggleReadyToStart)testTransporter.lastSent;
        Assert.True(sentPacket.newStatus);
    }

    [Fact]
    public void Test_IsNotReady_PacketGetsSent()
    {
        // Arrange
        var client = new Client(testTransporter);

        // Act
        client.IsNotReady();

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(24, testTransporter.lastSent.id);
        var sentPacket = (ToggleReadyToStart)testTransporter.lastSent;
        Assert.False(sentPacket.newStatus);
    }

    [Fact]
    public void Test_SendMessage_SendMessagePacketToServerWithMessage()
    {
        // Arrange
        var client = new Client(testTransporter);
        string payload = "Hello world!";

        // Act
        client.SendMessage(payload);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(34, testTransporter.lastSent.id);
        var sentPacket = (SendMessage)testTransporter.lastSent;
        Assert.Equal(payload, sentPacket.message);
    }

    [Fact]
    public void Test_ChangeGameSettings_SendPacketToServerWithSettings()
    {
        // Arrange
        var client = new TestClient(1, 2, null, testTransporter);
        var payload = new ChangeGameSettings();
        payload.settings = "GameMode: TurnBased";

        // Act 
        client.ChangeGameSettings(payload.settings);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(26, testTransporter.lastSent.id);
        var sentPacket = (ChangeGameSettings)testTransporter.lastSent;
        Assert.Equal(payload.settings, sentPacket.settings);
    }
    [Fact]
    public void Test_StartGame_SendStartGamePacketToServer()
    {
        // Arrange
        var client = new TestClient(1, 2, null, testTransporter);

        // Act
        client.StartGame();

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(22, testTransporter.lastSent.id);
    }
    [Fact]
    public void Test_CreateLobby_SendCreateLobbyPacketToServer_InspectServerResponse()
    {
        // Arrange
        var client = new Client(testTransporter);

        // Act
        client.CreateLobby();
        // mocking until we have a concrete implementation
        var acceptedPackageMock = new Mock<IAccepted>();
        acceptedPackageMock.Setup(p => p.id).Returns(3);

        testTransporter.ReceivePacket(acceptedPackageMock.Object);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(12, testTransporter.lastSent.id);
        Assert.True(testTransporter.hasReceived);
        Assert.Equal(3, testTransporter.lastReceived.id);
    }
    [Fact]
    public void Test_CreateGame_SendsCorrectGameName()
    {
        // Arrange
        var client = new Client(testTransporter);
        string gameName = "Chess";

        // Act
        client.CreateGame(gameName);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(69, testTransporter.lastSent.id);
        var sentPacket = (CreateGame)testTransporter.lastSent;
        Assert.Equal(gameName, sentPacket.gameName);
    }
    [Fact]
    public void Test_DeleteGame_SendsCorrectGameName()
    {
        // Arrange
        var client = new Client(testTransporter);
        string gameName = "Chess";

        // Act
        client.DeleteGame(gameName);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(36, testTransporter.lastSent.id);
        var sentPacket = (DeleteGame)testTransporter.lastSent;
        Assert.Equal(gameName, sentPacket.gameName);
    }
}