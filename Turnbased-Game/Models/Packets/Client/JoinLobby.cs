namespace Turnbased_Game.Models.Packets.Client;

public class JoinLobby : IPackage
{
    public byte PacketId => 14;

    public byte lobbyId;
}