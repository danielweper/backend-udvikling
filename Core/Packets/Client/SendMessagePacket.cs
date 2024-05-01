namespace Core.Packets.Client;

public class SendMessagePacket : IPacket
{
    public PacketType type => PacketType.SendMessage;
    public byte senderId { get; set; }
    public string message { get; set; }
}