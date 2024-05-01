namespace ServerLogic;

public class GameSettings(GameType gameType)
{
    public GameType GameType { get; init; } = gameType; // Once a GameType has been selected, it will remain like that for the whole game. 
    public string settings { get; set; }
}