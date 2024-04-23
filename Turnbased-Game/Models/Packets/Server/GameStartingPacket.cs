using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public class GameStartingPacket(DateTime timestamp) : IPackage
{
    public DateTime Timestamp { get; } = timestamp;
}