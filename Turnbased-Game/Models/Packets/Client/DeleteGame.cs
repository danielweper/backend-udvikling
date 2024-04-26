namespace Turnbased_Game.Models.Packets.Client;

public class DeleteGame : IPackage
{
    public byte PacketId { get; } = 36;
    public string gameName { get; set; }
    
}