using Turnbased_Game.Models.Client;
using Turnbased_Game.Models.Packages.Server;
using IHost = Turnbased_Game.Models.Client.IHost;

namespace Turnbased_Game.Models.ServerClasses;

public class Lobby
{
    public IHost Host { get; }
    public List<IParticipant> Participants;
    public List<IGame> Games;
    public int maxPlayerCount { get;}
    public byte Id { get; }

    public Lobby(IHost Host, byte id)
    {
        this.Host = Host;
        this.Id = id;
    }
    public ILobbyInfo GetInfo()
    {
        return new LobbyInfo(Id,Participants.Count,maxPlayerCount, Host);
    }

    public void AddPlayerToLobby(IParticipant participant)
    {
        Participants.Add(participant);
    }
}