using Turnbased_Game.Models.Packets.Server;
using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Server;

public interface IServer : IServerPackage
{
    Task ReceiveMessage(string user, string message);
    Task Acknowledge(AcknowledgedPacket message);
    Task Accepted(AcceptedPacket content);
    Task Denied(DeniedPacket content);
    Task InvalidRequest(IInvalidRequest content);

    Task UserMessage(IUserMessage content);
    Task SystemMessage(ISystemMessage content);
}