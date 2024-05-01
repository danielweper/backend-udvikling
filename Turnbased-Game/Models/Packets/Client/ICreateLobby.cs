namespace Turnbased_Game.Models.Packets.Client;

public class CreateLobby: IPackage
{
   public byte id { get; } = 12;
}