using Core.Model;

namespace Core.Packets.Server;

public class GameSettingsChangedPacket(IGameSettings newSettings) : IPacket
{
    public PacketType Type => PacketType.GameSettingsChanged;
    public readonly IGameSettings NewSettings = newSettings;
}