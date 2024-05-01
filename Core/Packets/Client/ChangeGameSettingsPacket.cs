namespace Core.Packets.Client;

public class ChangeGameSettingsPacket : IPacket
{
    public PacketType type => PacketType.ChangeGameSettings;
    public string settings { get; set; }
}