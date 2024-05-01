namespace Core.Packets.Client;

public class KickPlayerPacket : IPacket
{
    public PacketType type => PacketType.KickPlayer;
    public byte playerId { get; set; }
    public string reason { get; set; }
}