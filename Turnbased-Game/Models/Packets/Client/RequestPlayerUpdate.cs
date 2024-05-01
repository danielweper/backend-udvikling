namespace Turnbased_Game.Models.Packets.Client;

public class RequestPlayerUpdate : IPackage
{
    public byte PacketId => 28;
    public IPlayerProfile newProfile;
}