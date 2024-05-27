namespace Core.Packets.Client;

public class SubmitTurnPacket(char turnInfo) : IPacket
{
    public PacketType Type => PacketType.SubmitTurn;

    public readonly char TurnInfo = turnInfo;
}