namespace Core.Packets.Server;

public class GameSettingsChangedPacket : IPacket
{
    public PacketType type => PacketType.GameSettingsChanged;
}