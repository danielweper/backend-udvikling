namespace Core.Packets.Client;

public class RequestPlayerUpdate : IPacket
{
    public PacketType type => PacketType.RequestPlayerUpdate;

    //public IPlayerProfile newProfile;
}