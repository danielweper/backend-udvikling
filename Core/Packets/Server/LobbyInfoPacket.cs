namespace Core.Packets.Server;

public class LobbyInfoPacket(string info) : IPacket
{
    public PacketType Type => PacketType.LobbyInfo;
    public readonly string Info = info;
}