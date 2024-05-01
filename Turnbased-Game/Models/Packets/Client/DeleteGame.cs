namespace Turnbased_Game.Models.Packets.Client;

public class DeleteGame : IPackage
{
    public byte id { get; } = 36;
    public string gameName { get; set; }
    
}