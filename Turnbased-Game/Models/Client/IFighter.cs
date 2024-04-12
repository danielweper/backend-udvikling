using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Client;

public interface IFighter
{
    public event Func<bool> BattleIsOver;
    public event Func<string> TurnIsOver;

    public void SubmitTurn(string turn);
}