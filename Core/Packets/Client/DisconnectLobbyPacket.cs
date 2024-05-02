namespace Core.Packets.Client;

public class DisconnectLobbyPacket : IPacket
{
    public PacketType Type => PacketType.DisconnectLobby;
}