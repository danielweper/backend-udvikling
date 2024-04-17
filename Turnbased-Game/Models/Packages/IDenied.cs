namespace Turnbased_Game.Models.Packages;

public interface IDenied: IPackage
{
    public int RequestId { get; }
}