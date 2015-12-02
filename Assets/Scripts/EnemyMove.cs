using UnityEngine;
using System.Collections;

public class EnemyMove : Unit {
	
	//destination for ghost movements
	int refreshPath = 0;
	public Transform target;
	public float speed;
	Vector3[] path;
	Vector3 currentWaypoint;
	int targetIndex;


	void Start(){
		PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);	
	}
	
	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	IEnumerator FollowPath() {
		currentWaypoint = path [0];
		while (true) {
			if (transform.position == currentWaypoint){
				targetIndex++;
				if(targetIndex >= path.Length){
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards (transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;
		}
	}
	
	
	void FixedUpdate () {
		if (refreshPath == 8) {
			PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
			refreshPath=0;
		} else {
			refreshPath++;
		}
		
		//Vector2 dir = currentWaypoint - transform.position;
		//GetComponent<Animator>().SetFloat("DirX", dir.x);
		//GetComponent<Animator>().SetFloat("DirY", dir.y);
	}

	//collects treasure!!
	void OnTriggerEnter2D(Collider2D co){
		
		if (co.name == "treasure1")
			Destroy (co.gameObject);
		
	}
	
}
