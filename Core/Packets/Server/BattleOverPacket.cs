namespace Core.Packets.Server;

public class BattleOverPacket : IPacket
{
    public PacketType Type => PacketType.BattleIsOver;
}