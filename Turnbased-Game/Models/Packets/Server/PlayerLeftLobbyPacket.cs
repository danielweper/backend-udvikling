namespace Turnbased_Game.Models.Packets.Server;

public class PlayerLeftLobbyPacket(byte id)
{
    public byte id { get; private set; } 
}