namespace Core.Packets.Client;

public class SubmitTurnPacket(string turnInfo) : IPacket
{
    public PacketType Type => PacketType.SubmitTurn;

    public readonly string TurnInfo = turnInfo;
}