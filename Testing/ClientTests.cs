using Moq;
using ClientLogic;
using Core.Packets;
using Core.Packets.Client;
using Core.Packets.Shared;


namespace Testing;

public class ClientTests
{
    TestTransport testTransporter = new TestTransport();

    [Fact]
    public void Test_SendPackage_UpdatesTheLastPackageId()
    {
        // Arrange
        var client = new TestClient();
        var packageMock = new Mock<IPacket>();
        var packageId = PacketType.LobbyCreated;
        
        // Set up mock behavior
        packageMock.Setup(p => p.Type).Returns(packageId);

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
        if (testTransporter.lastSent != null)
        {
            Assert.Equal(32, (byte)testTransporter.lastSent.Type);
            var sentPacket = (SubmitTurnPacket)testTransporter.lastSent;
            Assert.Equal(payload, sentPacket.TurnInfo);
        }
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
        Assert.Equal(16, (byte)testTransporter.lastSent.Type);
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
        Assert.Equal(14, (byte)testTransporter.lastSent.Type);
        var sentPacket = (JoinLobbyPacket)testTransporter.lastSent;
        Assert.Equal(lobbyId, sentPacket.LobbyId);
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
        if (testTransporter.lastSent != null) Assert.Equal(16, (byte)testTransporter.lastSent.Type);
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
        if (testTransporter.lastSent == null) return;
        Assert.Equal(24, (byte)testTransporter.lastSent.Type);
        var sentPacket = (ToggleReadyToStartPacket)testTransporter.lastSent;
        Assert.True(sentPacket.NewStatus);
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
        if (testTransporter.lastSent == null) return;
        Assert.Equal(24, (byte)testTransporter.lastSent.Type);
        var sentPacket = (ToggleReadyToStartPacket)testTransporter.lastSent;
        Assert.False(sentPacket.NewStatus);
    }

    [Fact]
    public void Test_SendMessage_SendMessagePacketToServerWithMessage()
    {
        // Arrange
        var client = new Client(testTransporter);
        var payload = "Hello world!";

        // Act
        client.SendMessage(payload);

        // Assert
        Assert.True(testTransporter.hasSent);
        Assert.Equal(34, (byte)testTransporter.lastSent.Type);
        var sentPacket = (SendMessagePacket)testTransporter.lastSent;
        Assert.Equal(payload, sentPacket.Message);
    }

    [Fact]
    public void Test_ChangeGameSettings_SendPacketToServerWithSettings()
    {
        // Arrange
        var client = new TestClient(1, 2, null, testTransporter);
        var payload = new ChangeGameSettingsPacket("GameMode: TurnBased");
        //payload.Settings = "GameMode: TurnBased";

        // Act 
        client.ChangeGameSettings(payload.Settings);

        // Assert
        Assert.True(testTransporter.hasSent);
        if (testTransporter.lastSent == null) return;
        Assert.Equal(26, (byte)testTransporter.lastSent.Type);
        var sentPacket = (ChangeGameSettingsPacket)testTransporter.lastSent;
        Assert.Equal(payload.Settings, sentPacket.Settings);
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
        Assert.Equal(22, (byte)testTransporter.lastSent.Type);
    }
    [Fact]
    public void Test_CreateLobby_SendCreateLobbyPacketToServer_InspectServerResponse()
    {
        // Arrange
        var client = new Client(testTransporter);

        // Act
        client.CreateLobby();
        // mocking until we have a concrete implementation
        var acceptedPackageMock = new Mock<IPacket>();
        acceptedPackageMock.Setup(p => p.Type).Returns(PacketType.Accepted);

        testTransporter.ReceivePacket(acceptedPackageMock.Object);

        // Assert
        Assert.True(testTransporter.hasSent);
        if (testTransporter.lastSent != null) Assert.Equal(PacketType.CreateLobby, testTransporter.lastSent.Type);
        Assert.True(testTransporter.hasReceived);
        if (testTransporter.lastReceived != null) Assert.Equal(PacketType.Accepted, testTransporter.lastReceived.Type);
    }
    /*TODO - Implement the CreateGame method
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
    //TODO - Implement the DeleteGame method
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
    }*/
}