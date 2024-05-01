namespace Core.Packets.Shared;

public class DeniedPacket : IPacket
{
    public PacketType type => PacketType.Denied;
}