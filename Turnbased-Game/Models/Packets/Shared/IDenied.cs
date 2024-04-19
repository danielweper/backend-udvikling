namespace Turnbased_Game.Models.Packets.Shared;

public interface IDenied: IPackage
{
    public int RequestId { get; }
}