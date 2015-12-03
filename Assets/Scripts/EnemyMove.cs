using UnityEngine;
using System.Collections;

public class EnemyMove : Unit {


	public Animator animator;
	
	//destination for ghost movements
	int refreshPath = 0;
  public float sightRadius;
	public Transform target;
  public GameObject[] treasureTiles, oceanTiles;
  GameObject player;
  float turnTime;
  bool turnStarted;
	void Start(){
                target = new GameObject().transform;
                target.transform.position = transform.position;
                oceanTiles = GameObject.FindGameObjectsWithTag("Ocean");
                treasureTiles = GameObject.FindGameObjectsWithTag("Treasure");
                player = GameObject.FindGameObjectWithTag("Player");
                turnActive = false;
                turnStarted = false;
				animator = GetComponentInChildren<Animator> ();
	}
  void Update()
  {
    if(turnActive && movesLeft > 0 && !turnStarted)
    {
      turnTime = 0;
      var i = (int) ((oceanTiles.Length-1) * Random.value);
      target.position = oceanTiles[i].transform.position;
      treasureTiles = GameObject.FindGameObjectsWithTag("Treasure");
      foreach(var t in treasureTiles)
      {
        if(Mathf.Abs((t.transform.position - transform.position).magnitude) < sightRadius)
          target.position = t.transform.position;
      }
      if(player == null)
        player = GameObject.FindGameObjectWithTag("Player");
       if(Mathf.Abs((player.transform.position - transform.position).magnitude) < sightRadius)
          target.position = player.transform.position;
       PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
       turnStarted = true;
    }
    if(turnStarted && turnActive)
    {
	  
      turnTime += Time.deltaTime;
	  //this sets the parameters for the transitions between the animations states
	  //testing the animation update
	  Vector3 direction = target.position - transform.position;
	  animator.SetFloat ("DirX",direction.x);
	  animator.SetFloat ("DirZ",direction.z);
      if(turnTime > 5)
        EndTurn();
    }
    else
    {
      turnStarted = false;
    }


		
  }

	//collects treasure!!
	void OnTriggerEnter(Collider co){
		
		if (co.gameObject.tag == "Treasure")
                {
                  gold += co.gameObject.GetComponent<Treasure>().gold;
                   treasureTiles = GameObject.FindGameObjectsWithTag("Treasure");
                  Destroy (co.gameObject);
                }
                if (co.gameObject.tag == "Player" && canAttack)
                {
                  Attack(co.gameObject.GetComponent<Unit>());
                }
		
		
	}
	
}
