namespace Turnbased_Game.Models.Packets.Client;

public class CreateGame : IPackage
{
    public byte id { get; } = 69;
    public string gameName { get; set; }
}