namespace Turnbased_Game.Models.Server;

public readonly struct GameInfo(GameSettings gameSettings, List<Battle> battle)
{
    public GameSettings GameSettings => gameSettings;
    public List<Battle> Battles => battle;


    public GameInfo(Game game) : this(game.Settings, game.Battles)
    {
        
    }
}