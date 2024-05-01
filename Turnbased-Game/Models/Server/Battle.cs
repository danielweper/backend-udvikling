namespace Turnbased_Game.Models.Server;

public class Battle
{
    public byte BattleId { get; }

    // public List<Player> Fitghers { get; set; }
    public Player[] Fighters { get; } = new Player[2];

    // public List<Player> Winners { get; set; }
    public Player? Winner { get; private set; }

    public bool IsDone { get; set; }

    // public readonly List<Player> PlayersToExecuteTurn;
    public readonly List<Player> PlayersToExecuteTurn;


    public Battle(byte battleId)
    {
        BattleId = battleId;
        // Fighters = new List<Player>(2);
        // Winners = new List<Player>();
        IsDone = false;
        PlayersToExecuteTurn = Fighters.ToList();
    }

    public Battle(byte battleId, Player player1, Player player2)
    {
        BattleId = battleId;
        Fighters[0] = player1;
        Fighters[1] = player2;
        IsDone = false;
        PlayersToExecuteTurn = Fighters.ToList();
    }

    /*public void AddPlayer(Player player)
    {
        if (Figthers.Count >= 2) return;
        Figthers.Add(player);
        PlayersToExecuteTurn.Add(player);
        player.Health = 100;
    }*/


    public void ExecuteRound()
    {
        foreach (var player in Fighters)
        {
            ExecutePlayerTurn(player);
        }
    }

    private void ExecutePlayerTurn(Player player)
    {
        switch (player.FightAction)
        {
            case FightAction.Attack:
                Attack(player);
                break;
            case FightAction.Defense:
                Defend(player);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        player.ExecutedStatus = true;
    }

    private void Attack(Player attacker)
    {
        // var target = Fighters.Find(p => p.ParticipantId != attacker.ParticipantId);
        var target = Fighters[0] != attacker ? Fighters[0] : Fighters[1];

        // if (target == null) return;
        attacker.Attack = 10;
        var damage = attacker.Attack - target.Defense;
        if (damage > 0)
        {
            target.Health -= damage;
        }
    }

    private static void Defend(Player defender)
    {
        defender.Defense = 5;
    }

    public bool HasAWinner()
    {
        /*Winners = Figthers.Where(p => p.Health > 0).ToList();
        IsDone = Winners.Count == 1;
        return IsDone;*/

        if (Fighters[0].Health <= 0)
        {
            Winner = Fighters[0];
        }
        else if (Fighters[1].Health <= 0)
        {
            Winner = Fighters[1];
        }

        IsDone = true;

        return true;
    }

    public void UpdateExecutedTurn(Player player)
    {
        PlayersToExecuteTurn.Remove(player);
    }

    public List<Player> GetPlayersToExecuteTurn()
    {
        return PlayersToExecuteTurn;
    }

    public void EndGamePrematurely(Player loser)
    {
        IsDone = true;
        Winner = loser != Fighters[0] ? Fighters[0] : Fighters[1];
    }
}