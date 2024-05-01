namespace Core.Packets.Client;

public class SendMessage : IPacket
{
    public PacketType type => PacketType.SendMessage;
    public byte senderId { get; set; }
    public string message { get; set; }
}