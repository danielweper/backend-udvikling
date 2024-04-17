namespace Turnbased_Game.Models.Client;

public interface IChatter
{
    public event Action<byte, string> ReceivedUserMessage;
    public event Action<string> ReceivedSystemMessage;
    public event Action<byte, string> ReceivedMessage;

    public void SendMessage(string message);
}