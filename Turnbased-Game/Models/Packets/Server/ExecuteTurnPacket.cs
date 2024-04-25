namespace Turnbased_Game.Models.Packets.Server;

public class ExecuteTurnPacket(string turnInfo) : IServerPackage
{
    public byte Id => 31;
    
    public string TurnInfo { get; set; } = turnInfo;
}