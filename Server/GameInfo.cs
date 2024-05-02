using ServerLogic.Model.Fighting;

namespace ServerLogic;

public readonly struct GameInfo(Game game)
{
    public GameSettings GameSettings => game.Settings;
    public List<Battle> Battles => game.Battles;
}