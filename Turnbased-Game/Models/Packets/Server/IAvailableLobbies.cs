using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public interface IAvailableLobbies: IServerPackage
{
    public String[] LobbyInfo { get; set;}

    //public ILobbyInfo getLobbies();
    
}