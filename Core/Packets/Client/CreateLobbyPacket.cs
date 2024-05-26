namespace Core.Packets.Client;

public class CreateLobbyPacket(string playerName) : IPacket
{
    public PacketType Type => PacketType.CreateLobby;
    public readonly string HostName = playerName;
}