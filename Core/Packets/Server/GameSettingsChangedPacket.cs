namespace Core.Packets.Server;

public class GameSettingsChangedPacket : IPacket
{
    public PacketType Type => PacketType.GameSettingsChanged;
}