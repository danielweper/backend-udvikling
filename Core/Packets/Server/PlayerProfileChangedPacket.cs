namespace Core.Packets.Server;

public class PlayerProfileChangedPacket : IPacket
{
    public PacketType type => PacketType.ChangeGameSettings;
    public int PlayerId { get; }
    // public IPlayerProfile UpdatedProfile { get; set;}
}