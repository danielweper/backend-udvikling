using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public class PlayerProfileUpdatedInLobbyPacket(byte participantId, string displayName, PlayerProfile newPlayerProfile) : IPackage
{
    public byte ParticipantId { get; } = participantId;
    public string DisplayName { get; } = displayName;
    public PlayerProfile playerProfile { get; } = newPlayerProfile;
    public byte PacketId => 25;
}