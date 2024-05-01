namespace Core.Packets.Client;

public class SubmitTurn : IPacket
{
    public PacketType type => PacketType.SubmitTurn;

    public string turnInfo;
}