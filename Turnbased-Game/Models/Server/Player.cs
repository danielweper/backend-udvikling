using Turnbased_Game.Models.Client;

namespace Turnbased_Game.Models.Server;

public class Player(byte participantId)
{
    public byte ParticipantId { get; set; } = participantId;
    public PlayerRole Role { get; set; }
    public PlayerProfile Profile { get; set; }
    public bool ReadyStatus { get; set; }

    public enum PlayerRole
    {
        Spectator,
        Fighter
    }
}