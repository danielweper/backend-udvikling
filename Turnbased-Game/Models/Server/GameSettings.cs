namespace Turnbased_Game.Models.Server;

public class GameSettings(GameType gameType)
{
    private readonly GameType _type;

    public GameType GameType { get; init; } = gameType; // Once a GameType has been selected, it will remain like that for the whole game. 
    public string settings { get; set; }
}