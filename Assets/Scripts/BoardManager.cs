using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;
		
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
	
	public int columns = 10;
	public int rows = 10;
	public int scale = 10;

	public Count treasureCount = new Count (1, 5); //Lower and upper limit for our random number of food per level.

	public GameObject homeBase;
	public GameObject[] oceanTiles;
	public GameObject[] islandTiles;
	public GameObject[] treasureTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	
	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>(); //A list of possible locations to place tiles.
	
	//Clears our list gridPositions and prepares it to generate a new board.
	void InitialiseList()
	{
		gridPositions.Clear ();
		
		//Create decomposed world of Vector3 positions
		
		for (int x =1; x < columns -1; x++) {
			for(int z=1; z< rows-1;z++){
				gridPositions.Add (new Vector3(x*scale,0f,z*scale));
			}
		}
	}
	
	//Setup walls and floor of game board
	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;
		for (int x=-1; x < columns+1; x++) {
			for(int z=-1; z < rows+1; z++)
			{
				GameObject toInstantiate = oceanTiles[Random.Range(0,oceanTiles.Length)];
				
				if(x == -1 || x == columns || z == -1 || z == rows) // check if along an edge
					toInstantiate = outerWallTiles[Random.Range(0,outerWallTiles.Length)];
				
				GameObject instance = Instantiate(toInstantiate, new Vector3 (x*scale,0f,z*scale), Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
	}
	
	//Returns random Vector3 position from the decomposed gridPositions list.
	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex]; 
		randomPosition.y += 1f;
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}
	
	//Accepts array of gameObjects to choose from randomly, with min and max count
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);
		
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition(); //from the decomposed board, removed so not used again
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)]; //random tile from arg array
			Instantiate(tileChoice,randomPosition, Quaternion.identity);
		}
	}
	
	//Build scene according to level number
	public void SetupScene(int level)
	{
		BoardSetup ();
		InitialiseList ();
		//Layout Level Objects
		LayoutObjectAtRandom (treasureTiles, treasureCount.minimum, treasureCount.maximum);
		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		//Exit in upper right hand corner
		Instantiate (homeBase, new Vector3 ((columns - 1)*scale, 1f, (rows - 1)*scale), Quaternion.identity);
	}
}