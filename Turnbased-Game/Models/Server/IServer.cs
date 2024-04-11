namespace Turnbased_Game.Models.Server;

public interface IServer
{
    public void Ping();
    public void Acknowledge();
    public void Accepted();
    public void Denied();
    public void InvalidRequest(string natureOfError);

    public void UserMessage(IUserMessage content);
    public void SystemMessage(ISystemMessage content);
}