using Xunit;
using Moq;
using Turnbased_Game.Models.Packets;
using Turnbased_Game.Models.Packets.Client;

namespace Turnbased_Game.Models.Client
{
    public class ClientTests
    {
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
            Assert.Equal(packageMock.Object, client.lastPackage);
        }

        [Fact]
        public void Test_SubmitTurn_TurnInfoGetsThrough()
        {
            // Arrange
            var client = new TestClient();

            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            string payload = "turn: 1";
            
            // Act
            client.SubmitTurn(payload);

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(32, packageSent.id);
            SubmitTurn submitTurnPacket = (SubmitTurn)packageSent;
            Assert.Equal(payload, submitTurnPacket.turnInfo);
        }

        [Fact]
        public void Test_ListAvailableLobbies_PacketGetsSent()
        {
            // Arrange
            var client = new TestClient();

            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            
            // Act
            client.ListAvailableLobbies();

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(16, packageSent.id);
        }

        [Fact]
        public void Test_JoinLobby_JoiningTheCorrectLobby()
        {
            // Arrange
            var client = new TestClient();

            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            byte lobbyId = 42;
            
            // Act
            client.JoinLobby(lobbyId);

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(14, packageSent.id);
            JoinLobby readyPacket = (JoinLobby)packageSent;
            Assert.Equal(lobbyId, readyPacket.lobbyId);
        }

        [Fact]
        public void Test_DisconnectLobby_PacketGetsSent()
        {
            // Arrange
            var client = new TestClient();

            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            
            // Act
            client.ListAvailableLobbies();

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(18, packageSent.id);
        }

        [Fact]
        public void Test_IsReady_PacketGetsSent()
        {
            // Arrange
            var client = new TestClient();

            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            
            // Act
            client.IsReady();

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(24, packageSent.id);
            ToggleReadyToStart readyPacket = (ToggleReadyToStart)packageSent;
            Assert.True(readyPacket.newStatus);
        }

        [Fact]
        public void Test_IsNotReady_PacketGetsSent()
        {
            // Arrange
            var client = new TestClient();

            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            
            // Act
            client.IsNotReady();

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(24, packageSent.id);
            ToggleReadyToStart readyPacket = (ToggleReadyToStart)packageSent;
            Assert.False(readyPacket.newStatus);
        }

        [Fact]
        public void Test_SendMessage_SendMessagePacketToServerWithMessage()
        {
            // Arrange
            var client = new TestClient();

            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            string payload = "Hello world!";
            
            // Act
            client.SendMessage(payload);

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(34, packageSent.id);
            SendMessage messagePacket = (SendMessage)packageSent;
            Assert.Equal(payload, messagePacket.message);
            Assert.Equal(client.id , messagePacket.senderId);
        }

        [Fact]
        public void Test_ChangeGameSettings_SendPacketToServerWithSettings()
        {
            // Arrange
            var client = new TestClient();
            client.id = 1;
            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            var payload = new ChangeGameSettings();
            payload.settings = "GameMode: TurnBased";
           
            // Act 
            client.ChangeGameSettings(payload.settings);

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(11, packageSent.id);
            ChangeGameSettings changeGameSettings = (ChangeGameSettings)packageSent;
            Assert.Equal(payload.settings, changeGameSettings.settings);
            
        }
        [Fact]
        public void Test_StartGame_SendStartGamePacketToServer()
        {
            // Arrange
            var client = new TestClient();
            client.id = 1;
            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            
            // Act
            client.StartGame();

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(99, packageSent.id);
        }
        [Fact]
        public void Test_CreateLobby_SendCreateLobbyPacketToServer_InspectServerResponse()
        {
            // Arrange
            var client = new TestClient();
            IPackage? packageSent = null;
            IPackage? packageReceived = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            
            // Act
            client.CreateLobby();
            var acceptedPackageMock = new Mock<IAccepted>(); // mocking until we have a concrete implementation
            acceptedPackageMock.Setup(p => p.id).Returns(1);
            client.PackageReceived += delegate(IPackage package)
            {
                packageReceived = package;
            };
            client.ReceivePackage(acceptedPackageMock.Object);

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(3, packageSent.id);
            Assert.NotNull(packageReceived);
            Assert.Equal(1, packageReceived.id);
            Assert.IsAssignableFrom<IAccepted>(packageReceived);
            
        }
        [Fact]
        public void Test_CreateGame_SendsCorrectGameName()
        {
            // Arrange
            var client = new TestClient();
            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            string gameName = "Chess";
            
            // Act
            client.CreateGame(gameName);

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(69, packageSent.id);
            CreateGame createGame = (CreateGame)packageSent;
            Assert.Equal(gameName, createGame.gameName);
        }
        [Fact]
        public void Test_DeleteGame_SendsCorrectGameName()
        {
            // Arrange
            var client = new TestClient();
            IPackage? packageSent = null;
            client.PackageSent += delegate(IPackage package)
            {
                packageSent = package;
            };
            string gameName = "Chess";
            
            // Act
            client.DeleteGame(gameName);

            // Assert
            Assert.NotNull(packageSent);
            Assert.Equal(42, packageSent.id); 
            DeleteGame deleteGamePacket = (DeleteGame)packageSent;
            Assert.Equal(gameName, deleteGamePacket.gameName);
        }

    }
}   

