namespace Turnbased_Game.Models.Client;

public interface IClient : /*IChatter,*/ IFighter, IHost
{
    byte id { get; }
}