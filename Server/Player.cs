using Core.Packets.Server;
using Core.Model;

namespace ServerLogic;

public class Player(string displayName, byte participantId, IPlayerProfile playerProfile)
{
    public string DisplayName { get; set; } = displayName;
    public byte ParticipantId { get; set; } = participantId;
    public PlayerRole Role { get; set; } = PlayerRole.Spectator;
    public IPlayerProfile Profile { get; set; } = playerProfile;
    public bool ReadyStatus { get; set; } = false;
}