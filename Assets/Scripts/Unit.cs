using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {


  public Transform target;
  public Transform[] cannons;
	float speed = 20;
  public Vector3[] path;
	int targetIndex;
  public GameObject explosion, cannonball, treasure;
    public int movesLeft;
    public bool turnActive, canAttack, displayStats;
    public float health, healthMax;
    public int attackRating;
    public int defenseRating;
  public int gold;
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
          targetIndex = 0;
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
        
            screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            screenPos = new Vector2(screenPos.x + xOffset, Screen.height - screenPos.y + yOffset);                       
            GUI.BeginGroup(new Rect(screenPos.x, screenPos.y, healthBarSize.x, healthBarSize.y));
            GUI.Box(new Rect(0, 0, healthBarSize.x, healthBarSize.y), emptyHealth);
            GUI.BeginGroup(new Rect(0, 0, healthBarSize.x * health/healthMax, healthBarSize.y));
            GUI.Box(new Rect(0, 0, healthBarSize.x, healthBarSize.y), fullHealth);
            GUI.EndGroup();
            GUI.EndGroup();
        if (displayStats)
        {

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
        canAttack = false;
        StopCoroutine("FollowPath");
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
    public void Attack(Unit other)
    {
      //shoot cannon balls at other
      var dir = other.transform.position - transform.position;
      dir.y = 0;
      var dist = dir.magnitude;
      var angle = 60 * Mathf.Deg2Rad;
      dir.y = dist * Mathf.Tan(angle);
      var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude /  Mathf.Sin(2*angle)) * dir.normalized;
      StartCoroutine("ShootCannonBalls", vel);
      StartCoroutine("Explosion", other.transform.position);

      other.health -= attackRating/other.defenseRating;

      if(other.health <= 0)
      {
        var t = Instantiate(treasure);
        //t.GetComponent<Treasure>().gold = other.gold;
        Destroy(other.gameObject, 1);


      }
    }
  IEnumerator Explosion(Vector3 tar)
  {
    yield return new WaitForSeconds(1.5f);
    Destroy(Instantiate(explosion, tar, Quaternion.identity), 2.5f);
    yield return null;
  }
  IEnumerator ShootCannonBalls(Vector3 vel)
  {
    foreach(var t in cannons)
    {
      
      var ball = (GameObject)Instantiate(cannonball, t.position, Quaternion.identity);
      ball.GetComponent<Rigidbody>().velocity = vel;
      Destroy(ball, 2);
      yield return new WaitForSeconds(.5f);
    }
    yield return null;
  }
}

