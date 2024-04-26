namespace Turnbased_Game.Models.Packets.Client;

public class CreateGame : IPackage
{
    public byte PacketId { get; } = 69;
    public string gameName { get; set; }
}