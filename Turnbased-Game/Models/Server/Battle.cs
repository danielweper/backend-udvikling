namespace Turnbased_Game.Models.Server;

public class Battle
{
    public List<Player> players { get; set; }
    public List<Player> winners { get; set; }
    public bool isDone { get; set; }
    
    public Battle()
    {
        players = new List<Player>();
        winners = new List<Player>();
        isDone = false;
    }
}