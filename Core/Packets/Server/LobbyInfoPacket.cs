namespace Core.Packets.Server;

public class LobbyInfoPacket : IPacket
{
    public PacketType Type => PacketType.LobbyInfo;
}