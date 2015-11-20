using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	static int currentTurnCounter;
	static List<Turn> allObjects;

	public static TurnManager instance = null;
	
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);


		int currentTurnCounter = 0;
	}

	public void initObjects(List<GameObject> insertedObjects)
	{
		allObjects = new List<Turn>();

		foreach (GameObject o in insertedObjects)
			allObjects.Add (o.GetComponent<Turn> ());

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
