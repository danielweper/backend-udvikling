namespace Turnbased_Game.Models.Packages.Client;

public class KickPlayer: IPackage
{
    public byte playerId { get; set; }
    public string reason { get; set; }
    public byte id { get; } = 19;
}