using Turnbased_Game.Models.Client;

namespace Turnbased_Game.Models.Server;

public class Player(byte id)
{
    public byte id { get; set; } = id;
    public PlayerRole Role { get; set; }
    public PlayerProfile Profile { get; set; } 
    public bool ReadyStatus { get; set; }
    
    public enum PlayerRole
    { 
        Spectator,
        Fighter
    }
}