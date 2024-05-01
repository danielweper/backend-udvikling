namespace Turnbased_Game.Models.Packets.Shared;

public class DeniedPacket(string ackMessage, DateTime timestamp) : IPackage

{
    public string AckMessage { get; } = ackMessage;
    public DateTime Timestamp { get; } = timestamp;
    public byte PackageId => 4;
}