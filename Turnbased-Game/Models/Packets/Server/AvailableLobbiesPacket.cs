using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public class AvailableLobbiesPacket(List<LobbyInfo> lobbyInfoList) : IAvailableLobbies
{
    public byte PacketId => 19;
    public List<LobbyInfo> LobbyInfo { get; set; } = lobbyInfoList;
    public byte id { get; }
}