namespace Core.Packets.Client;

public class ChangeGameSettingsPacket : IPacket
{
    public PacketType Type => PacketType.ChangeGameSettings;
    public string settings { get; set; }
}