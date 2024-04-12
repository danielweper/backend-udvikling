using Turnbased_Game.Models.Packages;
using Turnbased_Game.Models.Packages.Server;

namespace Turnbased_Game.Models.Server;

public interface IServer : IServerPackage
{
    Task ReceiveMessage(string user, string message);
    Task Acknowledge(IAcknowledged message);
    Task Accepted();
    Task Denied();
    Task InvalidRequest(string natureOfError);

    Task UserMessage(IUserMessage content);
    Task SystemMessage(ISystemMessage content);
}