using UnityEngine;
public class Turn : MonoBehaviour
{
  public TurnManager tManager;
  public bool active;
  public int movesLeft;
  public void StartTurn()
  {
    active = true;
    movesLeft = 5;
  }
  public void EndTurn()
  {
    active = false;
    tManager.nextTurn();
  }
  
}
