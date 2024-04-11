using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Client;

public interface IChatter
{
    public event Func BattleIsOver;
    public event Func<string> TurnIsOver;

    public void SubmitTurn(string turn);
}