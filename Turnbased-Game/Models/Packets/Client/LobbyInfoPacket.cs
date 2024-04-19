using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.ServerClasses;

namespace Turnbased_Game.Models.Packets.Client;

public class LobbyInfoPacket : IPackage
{
    public LobbyInfo LobbyInfo { get; }

    public LobbyInfoPacket(LobbyInfo lobbyInfo)
    {
        LobbyInfo = lobbyInfo;
    }
}