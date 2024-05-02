namespace Core.Packets.Client;

public class KickPlayerPacket(byte playerId, string reason) : IPacket
{
    public PacketType Type => PacketType.KickPlayer;
    public readonly byte PlayerId = playerId;
    public readonly string Reason = reason;
}