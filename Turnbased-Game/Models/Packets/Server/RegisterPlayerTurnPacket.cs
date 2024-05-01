namespace Turnbased_Game.Models.Packets.Server;

public class RegisterPlayerTurnPacket(string turnInfo) : IServerPackage
{
    
    public string TurnInfo { get; set; } = turnInfo;
    public byte PackageId => 39;
}