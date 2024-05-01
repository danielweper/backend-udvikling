namespace Core.Packets.Client;

public class ListAvailableLobbiesPacket : IPacket
{
    public PacketType type => PacketType.ListAvailableLobbies;
}