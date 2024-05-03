namespace Core.Packets.Server;

public class AvailableLobbiesPacket(string lobbyInfo) : IPacket
{
    public PacketType Type => PacketType.AvailableLobbies;
    public string LobbyInfo => lobbyInfo;
} 