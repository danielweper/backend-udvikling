namespace Turnbased_Game.Models.Server;

public interface IGame
{
   public void ExecuteTurn(string turnInfo); 
   public void BattleOver();// event
   
}