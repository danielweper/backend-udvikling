namespace Core.Packets.Client;

public class CreateLobbyPacket : IPacket
{
    public PacketType Type => PacketType.CreateLobby;
    public string HostName;
}