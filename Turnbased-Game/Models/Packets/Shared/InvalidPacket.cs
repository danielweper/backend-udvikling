namespace Turnbased_Game.Models.Packets.Shared;

public class InvalidPacket(string ackMessage, DateTime timestamp): IPackage
{
    public string AckMessage { get; } = ackMessage;
    public DateTime Timestamp { get; } = timestamp;
}