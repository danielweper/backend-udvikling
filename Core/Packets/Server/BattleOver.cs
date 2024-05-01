namespace Core.Packets.Server;

public class BattleOver : IPacket
{
    public PacketType type => PacketType.BattleIsOver;
}