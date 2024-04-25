namespace Turnbased_Game.Models.Packets.Server;

public class PlayerLeftLobbyPacket(byte id)
{
    public byte PacketId => 17;
    public byte id { get; private set; } 
}