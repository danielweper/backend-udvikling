namespace Turnbased_Game.Models.Packets.Server;

public interface IRoleChanged: IServerPackage
{
    int PlayerId { get; }
    public IRole NewRole { get; set;}

}