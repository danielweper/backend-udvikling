namespace Core.Packets.Client;

public class DisconnectLobbyPacket : IPacket
{
    public PacketType type => PacketType.DisconnectLobby;
}