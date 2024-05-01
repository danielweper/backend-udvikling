namespace Turnbased_Game.Models.Packets.Client;

public class SubmitTurn : IPackage
{
    public byte PacketId => 32;
    public string turnInfo;
}