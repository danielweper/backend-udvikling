namespace Core.Packets;

public interface IPacket
{
    PacketType Type { get; }
}