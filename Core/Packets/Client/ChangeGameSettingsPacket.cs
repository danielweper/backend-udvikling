namespace Core.Packets.Client;

public class ChangeGameSettingsPacket(string settings) : IPacket
{
    public PacketType Type => PacketType.ChangeGameSettings;
    public readonly string Settings = settings;
}