using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	static int currentTurnCounter = 0;
	static List<GameObject> allObjects;

	public static TurnManager instance = null;
	
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	public void initObjects(List<GameObject> insertedObjects)
	{
		allObjects = insertedObjects;
	}

	public static void nextTurn()
	{
		currentTurnCounter++;
		if (currentTurnCounter > allObjects.Count) {
			currentTurnCounter = 0;
		}
	}

}
