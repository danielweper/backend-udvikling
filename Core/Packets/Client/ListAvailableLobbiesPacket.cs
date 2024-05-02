namespace Core.Packets.Client;

public class ListAvailableLobbiesPacket : IPacket
{
    public PacketType Type => PacketType.ListAvailableLobbies;
}