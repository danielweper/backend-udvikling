namespace ServerLogic;

public interface IGame
{
    public void ExecuteTurn(string turnInfo);

    public event Action BattleIsOver;
}