namespace Turnbased_Game.Models.Packets.Client;

public class JoinLobby : IPackage
{
    public byte id => 14;

    public byte lobbyId;
}