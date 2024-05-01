namespace Turnbased_Game.Models.Packets.Client;

public class SubmitTurn : IPackage
{
    public byte id => 32;

    public string turnInfo;
}