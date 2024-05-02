using Core.Model;

namespace Core.Packets.Server;

public class PlayerProfileChangedPacket(byte playerId, IPlayerProfile updatedProfile) : IPacket
{
    public PacketType Type => PacketType.ChangeGameSettings;
    public readonly byte PlayerId = playerId;
    public readonly IPlayerProfile UpdatedProfile = updatedProfile;
}