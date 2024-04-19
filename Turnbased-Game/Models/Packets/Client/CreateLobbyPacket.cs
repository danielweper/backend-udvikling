using Turnbased_Game.Models.Packets.Shared;

namespace Turnbased_Game.Models.Packets.Client;

public class CreateLobbyPacket : IPackage
{
    public byte RequestId { get; } = 3;
    public byte LobbyId { get; }

    public CreateLobbyPacket(byte lobbyId, byte id = 3)
    {
        RequestId = id;
        LobbyId = lobbyId;
    }
}