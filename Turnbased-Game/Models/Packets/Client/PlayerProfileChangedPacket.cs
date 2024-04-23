using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Client;

public class PlayerProfileChangedPacket(byte playerId, PlayerProfile newPlayerProfile) : IPackage
{
    public byte PlayerId => playerId;
    public PlayerProfile PlayerProfile => newPlayerProfile;
}