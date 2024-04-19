namespace Turnbased_Game.Models.Packets.Client;

public class ReceiveMessagePacket 
{
    public string Content { get; }
    public DateTime DateTime { get; }


    public ReceiveMessagePacket(string content, DateTime dateTime)
    {
        Content = content;
        DateTime = dateTime;
    }
}