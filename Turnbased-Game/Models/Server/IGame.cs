namespace Turnbased_Game.Models;

public interface IGame
{
   public void ExecuteTurn(string turnInfo); 
   public void BattleOver();// event
   
}