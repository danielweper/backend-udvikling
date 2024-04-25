namespace Turnbased_Game.Models.Packets.Server;

public class BattleIsOverPacket(bool battleFinished): IServerPackage
{
    public byte Id => 31;
    
    public bool BattleFinished { get; set; } = battleFinished;
}