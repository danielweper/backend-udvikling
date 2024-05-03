namespace Core.Packets.Client;

public class StartGamePacket(byte lobbyId, DateTime time) : IPacket
{
    public PacketType Type => PacketType.StartGame;
    public readonly byte LobbyId = lobbyId;
    public readonly DateTime Time = time;
}