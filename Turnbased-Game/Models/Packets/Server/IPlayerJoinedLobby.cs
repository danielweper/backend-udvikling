using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Server;

public interface IPlayerJoinedLobby : IPackage
{
    public int PlayerId { get; set; }
    public IPlayerProfile Profile{ get; set; }
}