using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Client;

public class GameInfoPacket(GameInfo gameInfo) : IPackage
{
    public byte PackageId => throw new NotImplementedException();
    public GameInfo GameInfo { get; } = gameInfo;
}