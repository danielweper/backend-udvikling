namespace Turnbased_Game.Models.Packets.Server;

public class ExecuteTurnsPacket(string turnInfo) : IServerPackage
{
    public byte PackageId => 31;
    
    public string TurnInfo { get; set; } = turnInfo;
}