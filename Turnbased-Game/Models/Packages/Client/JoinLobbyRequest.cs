using Turnbased_Game.Models.Packages.Shared;

namespace Turnbased_Game.Models.Packages.Client;

public class JoinLobbyRequest : IPackage
{
    public byte Id { get; } = 3;

    public JoinLobbyRequest(byte id)
    {
        this.Id = id;
    }
}