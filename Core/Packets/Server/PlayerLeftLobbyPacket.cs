namespace Core.Packets.Server;

public class PlayerLeftLobbyPacket : IPacket
{
    public PacketType type => PacketType.PlayerLeftLobby;
    public int PlayerId { get; }
}