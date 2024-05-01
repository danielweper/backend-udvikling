namespace Core.Packets.Client;

public class DisconnectLobby : IPacket
{
    public PacketType type => PacketType.DisconnectLobby;
}