using Xunit;
using Moq;
using Turnbased_Game.Models.Packages;
using Turnbased_Game.Models.Packages.Client;

namespace Turnbased_Game.Models.Client
{
    public class ClientTests
    {
        [Fact]
        public void Test_SendPackage_UpdatesTheLastPackageId()
        {
            // Arrange
            var client = new Client();
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

    }
}   

