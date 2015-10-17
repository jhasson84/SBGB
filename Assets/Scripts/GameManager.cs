using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public static GameManager instance = null;
	public BoardManager boardScript;
	public Text levelCounter;
	public Text shipGoldCounter;
	public Text totalGoldCounter;
	
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
		InitGame ();
	}
	
	
	void InitGame () 
	{
		boardScript.SetupScene (level);
		levelCounter.text = "LEVEL " + level;
	}
	
	// Update is called once per frame
	void Update() {

	}
	}