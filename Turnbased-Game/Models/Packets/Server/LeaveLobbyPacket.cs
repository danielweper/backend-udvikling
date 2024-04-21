namespace Turnbased_Game.Models.Packets.Server;

public class LeaveLobbyPacket(byte id)
{
    public byte id { get; private set; } 
}