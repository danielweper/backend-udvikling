namespace Core.Packets.Server;

public class AvailableLobbies : IPacket
{
    public PacketType type => PacketType.AvailableLobbies;
    public string[] LobbyInfo { get; set; }
}