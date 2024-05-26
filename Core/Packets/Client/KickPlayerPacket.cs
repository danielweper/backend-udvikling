namespace Core.Packets.Client;

public class KickPlayerPacket(string kickPlayerName, string reason, byte lobbyId) : IPacket
{
    public PacketType Type => PacketType.KickPlayer;
    public readonly string KickPlayerName = kickPlayerName;
    public readonly string Reason = reason;
    public readonly byte LobbyId = lobbyId;
}