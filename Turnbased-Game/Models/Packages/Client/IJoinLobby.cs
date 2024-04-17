using Turnbased_Game.Models.Packages.Shared;

namespace Turnbased_Game.Models.Packages.Client;

public interface IJoinLobby : IPackage
{
    public int LobbyId { get; }
}