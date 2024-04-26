using Turnbased_Game.Models.Packets.Client;

namespace Turnbased_Game.Models.Packets.Server;

public interface IGameSettingsChanged: IServerPackage
{
    public IChangeGameSettings NewSettings();   
}