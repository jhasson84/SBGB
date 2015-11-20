using UnityEngine;
public class Turn : MonoBehaviour
{
  public bool active, canAttack;
  public int movesLeft;
  public void StartTurn()
  {
    active = true;
    canAttack = true;
    movesLeft = 5;
  }
  public void EndTurn()
  {
    active = false;
    TurnManager.nextTurn();
  }
  
}
