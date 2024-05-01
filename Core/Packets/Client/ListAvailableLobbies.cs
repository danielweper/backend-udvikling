namespace Core.Packets.Client;

public class ListAvailableLobbies : IPacket
{
    public PacketType type => PacketType.ListAvailableLobbies;
}