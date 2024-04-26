using Turnbased_Game.Models.Packets.Shared;
using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public class LobbyInfoPacket(LobbyInfo lobbyInfo) : IPackage
{
    public byte PacketId => 13;
    public LobbyInfo LobbyInfo { get; } = lobbyInfo;
}