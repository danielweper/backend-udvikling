namespace Turnbased_Game.Models.Packages;

public interface IAcknowledged : IPackage
{
    public string AckMessage { get; set; }
    public DateTime Timestamp { get; set; }
}