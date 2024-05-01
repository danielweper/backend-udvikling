namespace Core.Packets.Server;

public class BattleOverPacket : IPacket
{
    public PacketType type => PacketType.BattleIsOver;
}