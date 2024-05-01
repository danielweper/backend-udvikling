namespace Core.Packets.Client;

public class RequestPlayerUpdatePacket : IPacket
{
    public PacketType type => PacketType.RequestPlayerUpdate;

    //public IPlayerProfile newProfile;
}