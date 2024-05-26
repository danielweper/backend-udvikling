namespace Core.Packets.Client;

public class SendMessagePacket(string message) : IPacket
{
    public PacketType Type => PacketType.SendMessage;
    public readonly string Message = message;
}