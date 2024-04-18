using Turnbased_Game.Models.Packages.Shared;

namespace Turnbased_Game.Models.Packages.Client;

public class CreateLobbyRequest : IPackage
{
    public byte Id { get; } = 3;

    public CreateLobbyRequest(byte id)
    {
        this.Id = id;
    }
}