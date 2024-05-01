namespace Core.Packets.Server;

public class RoleChanged : IPacket
{
    public PacketType type => PacketType.ChangeGameSettings;
}