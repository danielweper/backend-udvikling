using ServerLogic.Model.Fighting;

namespace ServerLogic;

public readonly struct GameInfo(Game game)
{
    public GameSettings GameSettings => game.Settings;
    public List<Battle> Battles => game.Battles;
    public bool BattleHasStarted => game.BattlesHasStarted;

    public override string ToString()
    {
        string battlesHasStartedText =
            BattleHasStarted ? "The game is currently in progress" : "The game has not started yet";

        return $"Game type: {game.Settings.GameType.ToString()}\n" +
               $"Number of battles: {Battles.Count}\n" +
               $"{battlesHasStartedText}";
    }
}