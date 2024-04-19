namespace Turnbased_Game.Models.Packets.Server;

public interface IPlayerProfileChanged: IServerPackage
{
    public int PlayerId { get; }   
    public IPlayerProfile UpdatedProfile { get; set;}
}