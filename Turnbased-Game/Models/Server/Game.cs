namespace Turnbased_Game.Models.Server;

public class Game
{
    public GameSettings settings { get; set; }
    public List<Battles> battles { get; set; }

    public Game()
    {
        settings = new GameSettings();
        battles = new List<Battles>();
    }
    
    
}