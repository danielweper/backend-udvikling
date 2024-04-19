namespace Turnbased_Game.Models.Packets.Client;

public class ToggleReadyToStart : IPackage
{
    public byte id => 24;

    public bool newStatus;
}