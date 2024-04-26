using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public class PlayerReadyStatusPacket(bool  newReadyStatus, byte playerId) : IPackage
{
    public bool NewReadyStatus { get; } = newReadyStatus;
    public byte PlayerId { get; } = playerId;
    public byte PacketId => 41;
}