using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;
	public GameObject HUD;
	public BoardManager boardScript;
	public TurnManager turnScript;
	public Grid gridScript;
	public Text levelCounter;
	public Text shipGoldCounter;
	public Text totalGoldCounter;
	
	bool menuActive = true;
	
	private int level = 1;

	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent<BoardManager> ();
		turnScript = GetComponent<TurnManager> ();
		gridScript = GetComponent<Grid> ();
	}

	void OnGUI () {
		if (menuActive) {
			// Make a background box
			GUI.Box (new Rect (10, 10, Screen.width - 10, Screen.height - 10), "Menu");

			if (GUI.Button (new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 10), "Start")) {
				instance.InitGame ();
				Instantiate (HUD);
				menuActive = false;
			}

			//Not sure how to quit the game
			if (GUI.Button (new Rect (Screen.width / 4, Screen.height / 4 + Screen.height / 10, Screen.width / 2, Screen.height / 10), "Quit")) {
				Application.Quit ();
			}
		} else {
			if (GUI.Button (new Rect (4, 4, 80, 40), "End")) {
				instance.gameOver();
				menuActive = true;
			}
		}
	}
	
	public void InitGame () 
	{
		boardScript.SetupScene (level);
		turnScript.initObjects (boardScript.getObjectList ());
		gridScript.gridWorldSize = new Vector2(boardScript.columns * boardScript.scale, boardScript.rows * boardScript.scale);
		gridScript.nodeRadius = boardScript.scale/2;
		gridScript.CreateGrid ();
		levelCounter.text = "LEVEL " + level;
	}

	public void gameOver()
	{
		level = 1;
		GameObject[] objects = GameObject.FindObjectsOfType<GameObject> ();
		foreach (GameObject o in objects) {
			if(!(o.name.Contains("GameManager") || o.name.Contains("EmptyObject")||o.name.Contains("Event")||o.name.Contains("Light")))
				Destroy(o);
		}

		boardScript = GetComponent<BoardManager> ();
		turnScript = GetComponent<TurnManager> ();
		gridScript = GetComponent<Grid> ();
	}
	

	}