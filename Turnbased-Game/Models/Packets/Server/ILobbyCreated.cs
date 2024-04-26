namespace Turnbased_Game.Models.Packets.Server;

public interface ILobbyCreated: IServerPackage
{
    public int LobbyId { get; set; }
}