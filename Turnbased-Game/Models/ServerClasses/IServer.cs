using Turnbased_Game.Models.Packages.Server;
using Turnbased_Game.Models.Packages.Shared;

namespace Turnbased_Game.Models.ServerClasses;

public interface IServer : IServerPackage
{
    Task ReceiveMessage(string user, string message);
    Task Acknowledge(Acknowledged message);
    Task Accepted(IAccepted content);
    Task Denied(IDenied content);
    Task InvalidRequest(IInvalidRequest content);

    Task UserMessage(IUserMessage content);
    Task SystemMessage(ISystemMessage content);
}