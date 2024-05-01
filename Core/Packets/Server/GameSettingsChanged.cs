namespace Core.Packets.Server;

public class GameSettingsChanged : IPacket
{
    public PacketType type => PacketType.GameSettingsChanged;
}