namespace ServerLogic.Model.Fighting;

public class Fighter(Player player)
{
    public readonly byte PlayerId = player.ParticipantId;
    public int Health { get; private set; } = 100;
    public int Attack { get; private set; } = 25;
    public int Defense { get; private set; } = 0;
    public FightTurn Turn { get; private set; }


    public event Action? OnDie;

    public void DoTurn(FightTurn turn)
    {
        Turn = turn;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Math.Max(0, damage - Defense);
        Health -= actualDamage;
        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Defend()
    {
        Defense++;
    }

    public void Kill()
    {
        Health = 0;
        OnDie?.Invoke();
    }
}
