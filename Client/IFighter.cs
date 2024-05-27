namespace ClientLogic;

public interface IFighter

{
    public event Action<bool> BattleIsOver; // check
    public event Action<string> TurnIsOver;

    public void SubmitTurn(char turn);
}