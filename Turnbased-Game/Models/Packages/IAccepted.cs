namespace Turnbased_Game.Models.Packages;

public interface IAccepted: IPackage
{
    public int RequestId { get; }
}