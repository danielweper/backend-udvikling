namespace Core.Packets.Server;

public class PlayerLeftLobbyPacket(string displayName) : IPacket
{
    public PacketType Type => PacketType.PlayerLeftLobby;
    public readonly string DisplayName = displayName;
}