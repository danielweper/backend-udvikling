using Turnbased_Game.Models.Server;

namespace Turnbased_Game.Models.Client;

public interface IFighter
{
    public event Action<bool> BattleIsOver;
    public event Action<string> TurnIsOver;

    public void SubmitTurn(string turn);
}