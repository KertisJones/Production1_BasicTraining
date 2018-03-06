using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public float turnDelay = .1f;
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	public int level = 1;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;

    public float timeBetweenSteps = 0.5f;
    private float timeLeft = 0.5f;
    public int steps = 0;
    public AudioClip marchSound1;
    public AudioClip marchSound2;
    public AudioClip marchSound3;
    public AudioClip marchSound4;

    // Use this for initialization
    void Awake () {
        timeLeft = timeBetweenSteps;

        if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager> ();
        if (level == 1)
		    InitGame ();
	}

	void OnLevelWasLoaded (int index)
	{
		//level++;

		//InitGame ();
	}

	public void InitGame()
	{
		doingSetup = true;

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);

		enemies.Clear ();
		boardScript.SetupScene (level);
	}

	private void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver()
	{
		levelText.text = "After " + level + " days, you starved.";
		levelImage.SetActive (true);
		enabled = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
        timeLeft -= Time.deltaTime;
        if (timeLeft < timeBetweenSteps * 0.9)
        {
            playersTurn = false;
        }
        if(timeLeft < 0)
        {
            timeLeft = timeBetweenSteps;
            steps += 1;
            Debug.Log("Step");

            switch (steps % 4)
            {
                case 0:
                    AudioSource.PlayClipAtPoint(marchSound4, new Vector3(3.5f, 3.5f, -10f));
                    //SoundManager.instance.PlaySingle(marchSound4);
                    break;
                case 1:
                    AudioSource.PlayClipAtPoint(marchSound1, new Vector3(3.5f, 3.5f, -10f));
                    //SoundManager.instance.PlaySingle(marchSound1);
                    break;
                case 2:
                    AudioSource.PlayClipAtPoint(marchSound2, new Vector3(3.5f, 3.5f, -10f));
                    //SoundManager.instance.PlaySingle(marchSound2);
                    break;
                case 3:
                    AudioSource.PlayClipAtPoint(marchSound3, new Vector3(3.5f, 3.5f, -10f));
                    //SoundManager.instance.PlaySingle(marchSound3);
                    break;
            }

            if (playersTurn || enemiesMoving || doingSetup)
                return;
            for (int currBaddy = 0; currBaddy < enemies.Count; currBaddy++)
            {
                if (enemies[currBaddy] == null)
                {
                    enemies.RemoveAt(currBaddy);
                }
            }

            MoveEnemies();

        }

    }

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add (script);
	}

	public void MoveEnemies()
	{
        enemiesMoving = true;
		//yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			//yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			//yield return new WaitForSeconds (enemies [i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}
