using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {


	public Transform target;
	float speed = 20;
	Vector3[] path;
	int targetIndex;
    public int movesLeft;
    public bool turnActive, canAttack, displayStats;
    public float health, healthMax;
    public int attackRating;
    public int defenseRating;
    public Vector2 healthBarSize, screenPos;
    public Texture2D emptyHealth;
    public Texture2D fullHealth;
    public float xOffset, yOffset;
	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() {
        Vector3 currentWaypoint = transform.position;
        if (path.Length > 0)
            currentWaypoint= path[0];

		while (true) {
			if ((transform.position - currentWaypoint).magnitude < 0.01) {
				targetIndex ++;
                movesLeft--;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;

		}
	}
    public IEnumerator ShowStats()
    {
        displayStats = true;
        yield return null;
    }
    void OnGUI()
    {
        if (displayStats)
        {
            screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            screenPos = new Vector2(screenPos.x + xOffset, Screen.height - screenPos.y + yOffset);                       
            GUI.BeginGroup(new Rect(screenPos.x, screenPos.y, healthBarSize.x, healthBarSize.y));
            GUI.Box(new Rect(0, 0, healthBarSize.x, healthBarSize.y), emptyHealth);
            GUI.BeginGroup(new Rect(0, 0, healthBarSize.x * health/healthMax, healthBarSize.y));
            GUI.Box(new Rect(0, 0, healthBarSize.x, healthBarSize.y), fullHealth);
            GUI.EndGroup();
            GUI.EndGroup();
        }
    }
    public void StartTurn()
    {
        movesLeft = 5;
        turnActive = true;
        canAttack = true;
    }
    public void EndTurn()
    {
        turnActive = false;
        TurnManager.nextTurn();
    }

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
    public void Attack(GameObject o)
    {
        
    }
}
