using Turnbased_Game.Models.Client;
using IHost = Turnbased_Game.Models.Client.IHost;

namespace Turnbased_Game.Models.ServerClasses;

public class Lobby
{
    public IHost Host { get; }
    public List<IParticipant> Participants;
    public List<IGame> Games;
    public byte Id { get; }

    public Lobby(IHost Host, byte id)
    {
        this.Host = Host;
        this.Id = id;
    }


    public void AddPlayerToLobby(IParticipant participant)
    {
        Participants.Add(participant);
    }
}