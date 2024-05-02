using System.Numerics;

namespace ServerLogic.Model.Fighting;

public class Battle
{
    public readonly byte BattleId;
    public Fighter[] Fighters { get; } = new Fighter[2];
    private bool[] isReady = new bool[2];

    public Battle(byte battleId, Player player1, Player player2)
    {
        BattleId = battleId;
        Fighters[0] = new Fighter(player1);
        Fighters[1] = new Fighter(player2);
        isReady[0] = false;
        isReady[1] = false;

        Fighters[0].OnDie += () => OnDone?.Invoke(player2);
        Fighters[1].OnDie += () => OnDone?.Invoke(player1);
    }

    public event Action<Player> OnDone;

    public void ExecuteRound()
    {
        List<ValueTuple<int, FightTurn>> sortedTurns = new();

        // Sort the actions by their speed
        for (int i = 0; i < Fighters.Length; i++)
        {
            sortedTurns.Add((i, Fighters[i].Turn));
        }
        sortedTurns.Sort((t1, t2) => t1.Item2.Speed - t2.Item2.Speed);

        // Execute each turn
        foreach (var turn in sortedTurns)
        {
            int fighterIndex = turn.Item1;

            var fighter = Fighters[fighterIndex];
            switch (turn.Item2.Action)
            {
                case FightAction.Attack:
                    Fighter target = Fighters[(fighterIndex + 1) % 2];
                    target.TakeDamage(fighter.Attack);
                    break;
                case FightAction.Defense:
                    fighter.Defend();
                    break;
                default:
                    throw new InvalidDataException();
            }

            isReady[fighterIndex] = false;
        }
    }

    public void UpdatePlayerTurn(Player player, FightTurn turn)
    {
        int fighterIndex = fighterIndexFromPlayer(player);

        Fighter fighter = Fighters[fighterIndex];
        fighter.DoTurn(turn);
        isReady[fighterIndex] = true;
    }

    public void PlayerForfeits(Player forfeiter)
    {
        int fighterIndex = fighterIndexFromPlayer(forfeiter);
        Fighter fighter = Fighters[fighterIndex];
        fighter.Kill();
    }

    private int fighterIndexFromPlayer(Player player) => (Fighters[0].PlayerId == player.ParticipantId ? 0 : 1);
}