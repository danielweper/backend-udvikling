namespace Core.Packets.Client;

public class SubmitTurnPacket : IPacket
{
    public PacketType type => PacketType.SubmitTurn;

    public string turnInfo;
}