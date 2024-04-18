namespace Turnbased_Game.Models.Packages.Shared;

public class Acknowledged : IPackage
{
    public Acknowledged(string ackMessage, DateTime timestamp)
    {
        AckMessage = ackMessage;
        Timestamp = timestamp;
    }

    public string AckMessage { get; }
    public DateTime Timestamp { get; }
}