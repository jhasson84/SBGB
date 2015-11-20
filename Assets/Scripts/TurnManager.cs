using UnityEngine;
using System;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {

	int currentTurnCounter = 0;
	List<GameObject> allObjects;

	public void initObjects(List<GameObject> insertedObjects)
	{
		allObjects = insertedObjects;
	}

	public void nextTurn()
	{
		currentTurnCounter++;
		if (currentTurnCounter > allObjects.Count ()) {
			currentTurnCounter = 0;
		}
	}

}
