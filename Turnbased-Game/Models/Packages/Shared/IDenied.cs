namespace Turnbased_Game.Models.Packages.Shared;

public interface IDenied: IPackage
{
    public int RequestId { get; }
}