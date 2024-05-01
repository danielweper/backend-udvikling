namespace Core.Packets.Server;

public class PlayerLeftLobby : IPacket
{
    public PacketType type => PacketType.PlayerLeftLobby;
    public int PlayerId { get; }
}