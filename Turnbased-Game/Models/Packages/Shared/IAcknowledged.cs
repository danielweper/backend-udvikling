namespace Turnbased_Game.Models.Packages.Shared;

public interface IAcknowledged : IPackage
{
    public string AckMessage { get; set; }
    public DateTime Timestamp { get; set; }
}