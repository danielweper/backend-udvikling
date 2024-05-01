using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public class BattleIsOverPacket(bool battleFinished, Player? battleWinner): IServerPackage
{
    public byte PackageId => 33;
    public bool BattleFinished { get; set; } = battleFinished;
    public Player? BattleWinner { get; } = battleWinner;
}