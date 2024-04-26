namespace Turnbased_Game.Models.Packets.Shared;

public class AcknowledgedPacket(string ackMessage, DateTime timestamp) : IPackage
{
    public string AckMessage { get; } = ackMessage;
    public DateTime Timestamp { get; } = timestamp;
    public byte PacketId => 2;
}