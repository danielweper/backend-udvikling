namespace Turnbased_Game.Models.Server;

public class Game(GameType gameType)
{
    public GameSettings settings { get; set; } = new GameSettings(gameType);
    public List<Battles> battles { get; set; } = new List<Battles>();
}