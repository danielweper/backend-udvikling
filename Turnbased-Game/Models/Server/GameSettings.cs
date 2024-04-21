namespace Turnbased_Game.Models.Server;

public class GameSettings(GameType gameType)
{
    private readonly GameType _type;

    public GameType GameType { get; init; } = gameType;
    public string settings { get; set; }
}