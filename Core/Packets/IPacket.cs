namespace Core.Packets;

public interface IPacket
{
    PacketType type { get; }
}