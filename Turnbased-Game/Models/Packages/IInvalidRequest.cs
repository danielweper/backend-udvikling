namespace Turnbased_Game.Models.Packages;

public interface IInvalidRequest: IPackage
{
    public int RequestId { get; }
    public string ErrorMessage { get; }
}