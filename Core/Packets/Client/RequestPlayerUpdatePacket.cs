using Core.Model;

namespace Core.Packets.Client;

public class RequestPlayerUpdatePacket(IPlayerProfile newProfile) : IPacket
{
    public PacketType Type => PacketType.RequestPlayerUpdate;

    public readonly IPlayerProfile NewProfile = newProfile;
}