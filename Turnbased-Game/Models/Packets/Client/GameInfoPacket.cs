using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Client;

public class GameInfoPacket(GameInfo gameInfo) : IPackage
{
    public GameInfo GameInfo { get; } = gameInfo;
}