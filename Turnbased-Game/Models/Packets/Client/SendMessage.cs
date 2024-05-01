namespace Turnbased_Game.Models.Packets.Client;

public class SendMessage : IPackage
{
    public byte senderId { get; set; }
    public string message { get; set; }
    public byte PacketId {get;} = 34;
}