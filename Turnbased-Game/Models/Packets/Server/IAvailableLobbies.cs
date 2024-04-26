using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public interface IAvailableLobbies: IServerPackage
{
    public List<LobbyInfo> LobbyInfo { get; set;}
}