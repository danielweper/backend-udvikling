namespace ServerLogic.Model.Fighting;

public readonly struct FightTurn(FightAction action)
{
    public readonly int Speed = action == FightAction.Defense ? 1 : 0;
    public readonly FightAction Action = action;
}
