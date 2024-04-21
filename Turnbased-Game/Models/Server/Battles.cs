namespace Turnbased_Game.Models.Server;

public class Battles
{
    public List<Player> players { get; set; }
    public List<Player> winners { get; set; }
    public bool isDone { get; set; }
    
    public Battles()
    {
        players = new List<Player>();
        winners = new List<Player>();
        isDone = false;
    }
}