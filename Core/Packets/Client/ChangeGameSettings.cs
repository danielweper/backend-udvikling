namespace Core.Packets.Client;

public class ChangeGameSettings : IPacket
{
    public PacketType type => PacketType.ChangeGameSettings;
    public string settings { get; set; }
}