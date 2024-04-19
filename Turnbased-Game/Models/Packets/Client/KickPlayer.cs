namespace Turnbased_Game.Models.Packets.Client;

public class KickPlayer: IPackage
{
    public byte playerId { get; set; }
    public string reason { get; set; }
    public byte id { get; } = 19;
}