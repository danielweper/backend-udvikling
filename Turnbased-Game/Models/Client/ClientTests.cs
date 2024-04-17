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
            Assert.Equal(34, packageSent.id);

            SendMessage messagePacket = (SendMessage)packageSent;
            Assert.Equal(payload, messagePacket.message);
            Assert.Equal(client.id , messagePacket.senderId);
        }
    }
}

