using UnityEngine;
using System.Collections;

public class EnemyMove : Unit {
	
	//destination for ghost movements
	int refreshPath = 0;
  public float sightRadius;
	public Transform target;
  GameObject[] treasureTiles, oceanTiles;
  GameObject player;
  float turnTime;
	void Start(){
                target = new GameObject().transform;
                target.transform.position = transform.position;
                oceanTiles = GameObject.FindGameObjectsWithTag("Ocean");
                treasureTiles = GameObject.FindGameObjectsWithTag("Treasure");
                player = GameObject.FindGameObjectWithTag("Player");
                
	}
  void Update()
  {
    if(turnActive && movesLeft > 0)
    {
      var i = (int) ((oceanTiles.Length-1) * Random.value);
      target.position = oceanTiles[i].transform.position;
      foreach(var t in treasureTiles)
      {
        if(Mathf.Abs((t.transform.position - transform.position).magnitude) < sightRadius)
          target.position = t.transform.position;
      }
       if(Mathf.Abs((player.transform.position - transform.position).magnitude) < sightRadius)
          target.position = player.transform.position;
       PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
       turnActive = false;
    }
    if(movesLeft < 1)
      EndTurn();
    if(turnTime > 5)
      EndTurn();
    turnTime += Time.deltaTime;

  }

	//collects treasure!!
	void OnTriggerEnter2D(Collider2D co){
		
		if (co.gameObject.tag == "Treasure")
                {
                  gold += co.gameObject.GetComponent<Treasure>().gold;
                  Destroy (co.gameObject);
                }
                if (co.gameObject.tag == "Player")
                {
                  Attack(co.gameObject.GetComponent<Unit>());
                }
		
		
	}
	
}
