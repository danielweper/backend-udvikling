namespace Turnbased_Game.Models.ServerClasses;

public interface IGame
{
   public void ExecuteTurn(string turnInfo); 
   public void BattleOver();// event
   
}