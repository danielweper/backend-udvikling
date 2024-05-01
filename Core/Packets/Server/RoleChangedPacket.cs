namespace Core.Packets.Server;

public class RoleChangedPacket : IPacket
{
    public PacketType type => PacketType.ChangeGameSettings;
}