namespace Turnbased_Game.Models.Packets.Client;

public class StartGame: IPackage
{
   public byte id { get; } = 22;
}