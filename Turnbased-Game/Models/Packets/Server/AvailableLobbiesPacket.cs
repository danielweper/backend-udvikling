using Turnbased_Game.Models.Packets.Server;

namespace Turnbased_Game.Models.Server;

public class AvailableLobbiesPacket(List<LobbyInfo> lobbyInfoList) : IAvailableLobbies
{
    public List<LobbyInfo> LobbyInfo { get; set; } = lobbyInfoList;
}