using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Client;

public class KickPlayerPacket(byte playerId, string reason) : IPackage
{
    public byte PackageId => throw new NotImplementedException();
    public byte PlayerId { get; } = playerId;
    public string Reason { get; } = reason;
}