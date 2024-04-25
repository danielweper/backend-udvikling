using Turnbased_Game.Models.Client;

namespace Turnbased_Game.Models.Server;

public class Player(string displayName, byte participantId, PlayerProfile playerProfile)
{
    public string DisplayName { get; set; } = displayName;
    public byte ParticipantId { get; set; } = participantId;
    public PlayerRole Role { get; set; }
    public PlayerProfile Profile { get; set; } = playerProfile;
    public bool ReadyStatus { get; set; }

    public enum PlayerRole
    {
        Spectator,
        Fighter
    }
}