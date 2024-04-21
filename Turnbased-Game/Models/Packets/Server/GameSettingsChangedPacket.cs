namespace Turnbased_Game.Models.Packets.Server;

public class GameSettingsChangedPacket(string settings)
{
    public string Settings { get; set; } = settings;
}