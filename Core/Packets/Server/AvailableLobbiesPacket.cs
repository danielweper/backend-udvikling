namespace Core.Packets.Server;

public class AvailableLobbiesPacket : IPacket
{
    public PacketType Type => PacketType.AvailableLobbies;
    public string[] LobbyInfo { get; set; }
}