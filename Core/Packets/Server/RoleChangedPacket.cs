namespace Core.Packets.Server;

public class RoleChangedPacket : IPacket
{
    public PacketType Type => PacketType.ChangeGameSettings;
}