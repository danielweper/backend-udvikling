using Core.Packets.Server;
using Core.Model;

namespace ServerLogic;

public class Player(string displayName, byte participantId, IPlayerProfile playerProfile)
{
    public string DisplayName { get; set; } = displayName;
    public byte ParticipantId { get; set; } = participantId;
    public PlayerRole Role { get; set; } = PlayerRole.Fighter;
    public IPlayerProfile Profile { get; set; } = playerProfile;
    public bool ReadyStatus { get; set; }
    public bool ExecutedStatus { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public FightAction FightAction { get; set; }

    public void ExecuteTurn(string turnInfo)
    {
        if (turnInfo == FightAction.Attack.ToString())
        {
            FightAction = FightAction.Attack;
        }
        else if (turnInfo == FightAction.Defense.ToString())
        {
            FightAction = FightAction.Defense;
        }
    }
}
public enum FightAction
{
    Attack,
    Defense
}