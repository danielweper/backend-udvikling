using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public class BattleIsOverPacket(bool battleFinished, List<Player> battleWinner): IServerPackage
{
    public byte PacketId => 33;
    public bool BattleFinished { get; set; } = battleFinished;
    public List<Player> BattleWinner { get; } = battleWinner;
}