namespace Turnbased_Game.Models.Packets.Client;

public class ChangeGameSettings: IPackage
{
    public string settings { get; set; }

    public byte id { get; } = 11;
}