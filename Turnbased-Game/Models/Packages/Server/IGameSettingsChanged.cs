using Turnbased_Game.Models.Packages.Client;

namespace Turnbased_Game.Models.Packages.Server;

public interface IGameSettingsChanged: IServerPackage
{
    public IChangeGameSettings NewSettings();   
}