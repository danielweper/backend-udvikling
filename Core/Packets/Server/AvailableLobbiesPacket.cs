namespace Core.Packets.Server;

public class AvailableLobbiesPacket : IPacket
{
    public PacketType type => PacketType.AvailableLobbies;
    public string[] LobbyInfo { get; set; }
}