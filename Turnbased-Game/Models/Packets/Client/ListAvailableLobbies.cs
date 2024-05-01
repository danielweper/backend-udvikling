namespace Turnbased_Game.Models.Packets.Client;

public class ListAvailableLobbies : IPackage
{
    public byte PacketId => 16;
}