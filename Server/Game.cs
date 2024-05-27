using ServerLogic.Model.Fighting;

namespace ServerLogic;

public class Game(GameType gameType)
{
    public GameSettings Settings { get; set; } = new GameSettings(gameType);
    public List<Battle> Battles { get; set; } = new List<Battle>();
    public bool BattlesHasStarted { get; set; } = false;

    public GameInfo GetInfo()
    {
        return new GameInfo(this);
    }

    public void CreateBattle(byte battleId, Player player1, Player player2)
    {
        Battles.Add(new Battle(battleId, player1, player2));
        Console.WriteLine("Added a battle");
        Console.WriteLine($"Battle count: {Battles.Count}");
    }

    public Battle? GetBattle(byte requestId)
    {
        foreach (var battle in Battles)
        {
            if (battle.BattleId == requestId)
            {
                return battle;
            }
        }

        return null;
    }
}