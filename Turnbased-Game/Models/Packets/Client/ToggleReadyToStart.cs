namespace Turnbased_Game.Models.Packets.Client;

public class ToggleReadyToStart : IPackage
{
    public bool newStatus;
    public byte PacketId => 24;
}