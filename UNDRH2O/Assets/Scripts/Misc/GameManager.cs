using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // The waterlevel
	public Transform water;

    // The Transform under which all leaks are placed
	public Transform leaksParent;
    [Tooltip("The minimum and maximum time between leak bursts")]
	public Vector2 burstRate;

    // The screen that is going to show up once the game is over
    public int leaksFixed = 0;
	public GameObject loseScreen;

    // The "life" of the basement
	[HideInInspector]
	public Stats roomStats;

	public AudioSource waterAmbience;
	public Vector2 wARange;
	public float wAVolume;
	public float wAMute;

    // All leaks in the scene
	public List<Leak> leaks;
    public List<Leak> activeLeaks;

    //Level manager is unique to each level, some may have one, others may not

    //The amount of time we have survived in the game
	float timer = 0f;
    // The amount of time until the next burst
	float burstTime;

    float maxHealth;
	bool gameOver = false;
    public bool IsGameOver { get { return gameOver; } }
    public int score = 0;

    [HideInInspector]
    public bool openingFinished = false;

    // Grabbed by all leaks to reference all possible UI sprites
    public Sprite[] LeakHealthUI;

    // How long should the room take to flush
    public float flushDuration;
    // keeps track of flush time progressed
    float flushCurrent;
    // Has the flush finished?
    bool flushFinished = false;

    public delegate void KillAllPipes();
    public static event KillAllPipes OnKillAllPipes;

	public AudioSource source;
	public AudioClip flushSound;
	public AudioClip music;


    void Start()
    {
        // Get a reference to our own stats
        roomStats = GetComponent<Stats>();
        maxHealth = roomStats.health;
		foreach (Leak leak in leaksParent.gameObject.GetComponentsInChildren<Leak> ())
		{
			leaks.Add (leak);
		}
        
    }

    public void NewLeak(Leak newLeak)
    {
        leaks.Add(newLeak);
    }

	void FixedUpdate ()
	{
        // If the player loses
		if (roomStats.health <= 0)
		{
			GameOver ();
		}

        // Run the leak system
		if (!gameOver && openingFinished) 
		{
			ManageLeaks ();
		}
			

        if(gameOver && !flushFinished)
        {
            Flush();

        }

		float step = (maxHealth - roomStats.health) / maxHealth;
		water.position = Vector3.Lerp (new Vector3 (water.position.x, 0f, water.position.z), new Vector3 (water.position.z, 2.25f, water.position.z), step);

		if (roomStats.health <= wARange.x) 
		{
			waterAmbience.volume = 1 - roomStats.health/wARange.x;
		}

		DebugControls ();
	}

	void ManageLeaks()
	{
		timer += Time.deltaTime;

		if (leaks.Count > 0)
		{
			if (timer > burstTime)
            {
                burstTime = timer + Random.Range(burstRate.x, burstRate.y);
                int index = Random.Range(0, leaks.Count);
                leaks [index].Burst();
                activeLeaks.Add(leaks[index]);
                leaks.RemoveAt(index);
			}
		}
	}

	void GameOver()
	{
		gameOver = true;
        if(OnKillAllPipes != null)
            OnKillAllPipes();
        

        // AUDIO HERE : Flush;
        // Turn on Particle Effect for the drips
		source.PlayOneShot(flushSound);
        

        
        //Change this to be UI that follows the player
		loseScreen.GetComponent<TextMesh> ().text = "GAME OVER\n" + ((int)Mathf.Round(timer) + score).ToString ()  + "  POINTS";
	}

    void Flush()
    {
        flushCurrent += Time.deltaTime;
        roomStats.health = Mathf.Lerp(0, roomStats.maxHealth, flushCurrent / flushDuration);
	

        if (flushCurrent >= flushDuration)
            flushFinished = true;
    }

	void DebugControls()
	{
		if (Input.GetKey(KeyCode.W))
		{
			roomStats.health -= 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			roomStats.health += 2;
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene (0);
		}
	}
}
