using Turnbased_Game.Models.Packets.Server;

namespace Turnbased_Game.Models.Server;

public class AvailableLobbiesPacket: IAvailableLobbies
{
    public string[] LobbyInfo { get; set; }

    public AvailableLobbiesPacket(string[] lobbyInfoList)
    {
        LobbyInfo = lobbyInfoList;
    }
}