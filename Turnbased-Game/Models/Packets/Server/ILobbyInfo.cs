using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Packets.Server;

public interface ILobbyInfo: IServerPackage
{
    public byte id { get; }
    public Player host { get; }
    public Player[] players { get; }
    public int maxPlayerCount { get; }
}