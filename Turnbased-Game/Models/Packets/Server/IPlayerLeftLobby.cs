namespace Turnbased_Game.Models.Packets.Server;

public interface IPlayerLeftLobby: IServerPackage
{
    public int PlayerId { get; }
}