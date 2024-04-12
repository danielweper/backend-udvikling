namespace Turnbased_Game.Models.Client;

public interface IChatter
{
    public event Func<byte, string> ReceivedUserMessage;
    public event Func<string> ReceivedSystemMessage;
    public event Func<byte, string> ReceivedMessage;

    public void SendMessage(string message);
}