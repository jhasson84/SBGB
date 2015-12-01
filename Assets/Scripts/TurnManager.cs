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

	public void initObjects(List<GameObject> insertedObjects)
	{
		allObjects = new List<Unit>();

		foreach (GameObject o in insertedObjects)
			if(o != null)
			allObjects.Add (o.GetComponent<Unit> ());

	}

	public static void nextTurn()
	{
		allObjects [currentTurnCounter].StartTurn ();
		currentTurnCounter++;
		if (currentTurnCounter > allObjects.Count) {
			currentTurnCounter = 0;
		}
	}

}
