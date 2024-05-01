namespace Turnbased_Game.Models.Packets.Shared;

public class AcceptedPacket(string accMessage, DateTime timestamp) : IPackage
{
    public string AccMessage { get; } = accMessage;
    public DateTime Timestamp { get; } = timestamp;
    public byte PackageId => 3;
}