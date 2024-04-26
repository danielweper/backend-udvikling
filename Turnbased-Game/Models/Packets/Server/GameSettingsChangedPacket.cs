namespace Turnbased_Game.Models.Packets.Server;

public class GameSettingsChangedPacket(string settings)
{
    public byte PacketId => 21;
    public string Settings { get; set; } = settings;
}