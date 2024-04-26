namespace Turnbased_Game.Models.Packets.Server;

public class RegisterPlaterTurnPacket(string turnInfo) : IServerPackage
{
    
    public string TurnInfo { get; set; } = turnInfo;
}