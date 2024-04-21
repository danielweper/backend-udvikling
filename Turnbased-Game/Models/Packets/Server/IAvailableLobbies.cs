namespace Turnbased_Game.Models.Packets.Server;

public interface IAvailableLobbies: IServerPackage
{
    public string[] LobbyInfo { get; set;}
}