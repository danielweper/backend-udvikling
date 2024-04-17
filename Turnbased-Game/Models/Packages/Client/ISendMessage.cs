namespace Turnbased_Game.Models.Packages.Client;

public interface ISendMessage: IPackage
{
    byte senderId { get; }
    string message { get; }
}