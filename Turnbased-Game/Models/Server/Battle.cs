namespace Turnbased_Game.Models.Server;

public class Battle
{
    public byte BattleId { get; }
    public List<Player> Figthers { get; set; }
    public List<Player> Winners { get; set; }
    public bool IsDone { get; set; }
    public readonly List<Player> PlayersToExecuteTurn;
    
    public Battle(byte battleId)
    {
        BattleId = battleId;
        Figthers = new List<Player>(2);
        Winners = new List<Player>();
        IsDone = false;
        PlayersToExecuteTurn = Figthers;
    }
    
    public void AddPlayer(Player player)
    {
        if (Figthers.Count >= 2) return;
        Figthers.Add(player);
        PlayersToExecuteTurn.Add(player);
        player.Health = 100;
    }
    
    public void ExecuteRound()
    {
        foreach (var player in Figthers)
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
        var target = Figthers.Find(p => p.ParticipantId != attacker.ParticipantId);

        if (target == null) return;
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
        Winners = Figthers.Where(p => p.Health > 0).ToList();
        IsDone = Winners.Count == 1;
        return IsDone;
    }
    public void UpdateExecutedTurn(Player player)
    {
        PlayersToExecuteTurn.Remove(player);
    }

    public List<Player> GetPlayersToExecuteTurn()
    {
        return PlayersToExecuteTurn;
    }
}