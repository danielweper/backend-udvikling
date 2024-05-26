namespace Core.Packets.Server;

public class LobbyChangedPacket(string info) : IPacket
{
    public PacketType Type => PacketType.LobbyChanged;
    public readonly string Info = info;
}