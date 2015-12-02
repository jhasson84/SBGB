using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	static int currentTurnCounter;
	static List<Unit> allObjects;

	public static TurnManager instance = null;
	
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);


		currentTurnCounter = 0;
	}

	public void initObjects()
	{
		allObjects = new List<Unit>();
                allObjects.Add(GameObject.FindWithTag("Player").GetComponent<Unit>());
                foreach(var o in GameObject.FindGameObjectsWithTag("Enemy"))
                  allObjects.Add(o.GetComponent<Unit>());
                foreach(var u in allObjects)
                  u.turnActive = false;
                nextTurn();
		//foreach (GameObject o in insertedObjects)
                  //if(o != null)
                          //allObjects.Add (o.GetComponent<Unit> ());
                

	}

	public static void nextTurn()
	{
		allObjects[currentTurnCounter].StartTurn ();
		currentTurnCounter++;
		if (currentTurnCounter >= allObjects.Count) {
			currentTurnCounter = 0;
		}
	}

}
