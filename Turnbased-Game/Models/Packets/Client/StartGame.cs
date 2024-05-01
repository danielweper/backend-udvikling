namespace Turnbased_Game.Models.Packets.Client;

public class StartGame : IPackage
{
   public byte PacketId { get; } = 22;
}