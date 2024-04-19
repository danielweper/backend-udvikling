namespace Turnbased_Game.Models.Packages.Client;

public class DisconnectLobby : IPackage
{
    public byte id => 24;

    public bool newStatus;
}