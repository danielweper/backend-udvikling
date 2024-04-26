namespace Turnbased_Game.Models.Packets.Server;

public class ExecuteTurnsPacket(string turnInfo) : IServerPackage
{
    public byte PacketId => 31;
    
    public string TurnInfo { get; set; } = turnInfo;
}