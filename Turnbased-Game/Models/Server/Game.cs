namespace Turnbased_Game.Models.Server;

public class Game(GameType gameType)
{
    public GameSettings settings { get; set; } = new GameSettings(gameType);
    public List<Battle> battles { get; set; } = new List<Battle>();

    public GameInfo GetInfo()
    {
        return new GameInfo(settings, battles);
    }
}