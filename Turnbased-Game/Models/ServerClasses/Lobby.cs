using Turnbased_Game.Models.Client;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace Turnbased_Game.Models.ServerClasses;

public class Lobby : ILobby
{
    public IHost Host { get; set; }
    public List<IParticipant> Participants;
    public List<IGame> Games;

    public void LobbyCreated()
    {
        throw new NotImplementedException();
    }

    public void LobbyInfo()
    {
        throw new NotImplementedException();
    }

    public void PlayerJoined()
    {
        throw new NotImplementedException();
    }

    public void PlayerLeft(int playerId)
    {
        throw new NotImplementedException();
    }

    public void PlayerInfo(int playerId)
    {
        throw new NotImplementedException();
    }

    public void ListAvailableLobbies()
    {
        throw new NotImplementedException();
    }

    public void GameCreated()
    {
        throw new NotImplementedException();
    }

    public void GameDeleted()
    {
        throw new NotImplementedException();
    }

    public void GameStarting(int gameId)
    {
        throw new NotImplementedException();
    }

    public void GameSettingsChanged(int gameId)
    {
        throw new NotImplementedException();
    }

    public void PlayerProfileChanged(int playerId)
    {
        throw new NotImplementedException();
    }

    public void RoleChangeRequest(int playerId, IRole requestedRole)
    {
        throw new NotImplementedException();
    }

    public void RoleChanged()
    {
        throw new NotImplementedException();
    }
}