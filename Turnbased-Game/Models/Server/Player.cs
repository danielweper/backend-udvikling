using Turnbased_Game.Models.Client;

namespace Turnbased_Game.Models.Server;

public class Player(string displayName, byte participantId)
{
    public string DisplayName { get; set; } = displayName;
    public byte ParticipantId { get; set; } = participantId;
    public PlayerRole Role { get; set; }
    public PlayerProfile Profile { get; set; }
    public bool ReadyStatus { get; set; }

    public Player(string displayName, byte participantId, PlayerProfile profile) : this(displayName, participantId)
    {
        Profile = profile;
    }

    public enum PlayerRole
    {
        Spectator,
        Fighter
    }
}