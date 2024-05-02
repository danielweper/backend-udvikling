namespace Core.Packets.Shared;

public class DeniedPacket : IPacket
{
    public PacketType Type => PacketType.Denied;
}