using Turnbased_Game.Models.Server;

namespace ClientLogic;

public interface IFighter

{
    public event Func<string> BattleIsOver; // check
    public event Func<string> TurnIsOver;

    public void SubmitTurn(string turn);
}