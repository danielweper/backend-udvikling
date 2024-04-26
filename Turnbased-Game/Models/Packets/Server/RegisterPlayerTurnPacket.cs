namespace Turnbased_Game.Models.Packets.Server;

public class RegisterPlayerTurnPacket(string turnInfo) : IPackage
{
    
    public string TurnInfo { get; set; } = turnInfo;
    public byte PacketId => 39
}