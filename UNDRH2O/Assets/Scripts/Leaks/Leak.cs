using UnityEngine;
using System.Collections;

public class Leak : ToolInteractable 
{

    [Tooltip("Click in Playmode to make this leak burst immediately")]
    public bool burstNow;
    [Tooltip("How much HP should the leak regain per second while able to regenerate")]
    public float regenRate;
    [Tooltip("Should this Leak be able to respawn once it has been fixed?")]
    public bool respawns;
    [Tooltip("How long should this leak take to burst again after being fixed?")]
    public float respawnTime;

	[HideInInspector]
	public enum State
	{ 
        ActiveFull,
        Active,
        Inactive,
        Dead,
        Start, // Starts leak on next CheckHealth()
        Standby,
        Kill
    }
    public State state = State.Standby;

    // Handle particle effects
    bool belowWater = false;
    Transform particleKiller;
	[SerializeField] protected GameObject[] particleSystems;

    protected bool waitingForRegen;
    protected GameManager gm;

    protected SpriteRenderer healthUI;
    protected int healthUIIndex;

    SphereCollider triggerSphere;
    float storedRad;

    // Points Prefab
    public GameObject pointsPrefab;
    public int pointsForFixing;
    public AudioClip pointClip;

    protected AudioSource source;
	public AudioSource loopSource;
	public AudioClip burst;
	public AudioClip burstSpray;



    void OnEnable()
    {
        GameManager.OnKillAllPipes += KillPipe;
    }

    void OnDisable()
    {
        GameManager.OnKillAllPipes -= KillPipe;
    }

    public override void Start ()
	{
        base.Start();
		gm = FindObjectOfType<GameManager> ();
        source = GetComponent<AudioSource>();
        particleKiller = gm.water.GetChild(0);
        triggerSphere = triggerZone as SphereCollider;
        healthUI = transform.FindChild("Health UI").GetComponent<SpriteRenderer>();
    }
	

	public override void Update ()
	{
		if (burstNow || state == State.Start) //Implementation allowing for in editor overload
		{
			Burst ();
			burstNow = false;
		}

        if (state == State.Standby)
            return;

        //Do we need to flip the water?
        if ((!belowWater && particleKiller.position.y > transform.position.y) || (belowWater && particleKiller.position.y < transform.position.y))
        {
            belowWater = particleKiller.position.y > transform.position.y;
            EnableProperParticleSystems();
        }

        CheckHealth();

        HandleHealthUI();

        if (waitingForRegen == false)
			stats.health += (regenRate * Time.deltaTime);
	}


	protected virtual IEnumerator Dead()
	{
        if (!respawns)
            StopCoroutine(Dead());

        source.Stop();
        loopSource.Stop();

        //Have UI and sound to indicate 
        GameObject pointsUI = (GameObject)Instantiate(pointsPrefab, transform.position + Vector3.up * 0.1f, transform.rotation, transform);
        pointsUI.GetComponentInChildren<TextMesh>().text = "+" + pointsForFixing;
        AudioUtilities.AudioFlair(source);
        source.PlayOneShot(pointClip, 0.5f);

        if(!keepGhostOnDeath)
        {
            foreach(GameObject ghost in ghostTool)
            {
                Destroy(ghost);
            }
            ghostTool.Clear();
        }

        gm.score += pointsForFixing;

		yield return new WaitForSeconds (respawnTime);
		waitingForRegen = false;
		stats.health = 1.0f;
	}


	public virtual void Burst()
	{
		if (state == State.Standby || state == State.Dead || state == State.Start)
        {
			StopCoroutine ("Dead");
			waitingForRegen = false;
			stats.dead = false;
            stats.health = stats.maxHealth;
            recievingTools = true;
            state = State.ActiveFull;

            if (!keepGhostOnDeath)
            {
                DestroyAllGhosts();
            }

            EnableProperParticleSystems();

            // AUDIO HERE : Leak bursting
			source.PlayOneShot(burst, 0.2f);
            loopSource.clip = burstSpray;
			loopSource.Play();


		}
        else
        {
			Debug.Log (name + " was unable to burst because it is already alive");
		}
	}

    void KillPipe()
    {
        StopAllCoroutines();
        source.Stop();
        loopSource.Stop();
        waitingForRegen = true;
        stats.dead = true;
        recievingTools = false;
        state = State.Kill;
        stats.health = 0;
        EnableProperParticleSystems();
    }


    public override void BeginInteraction(Tool tool)
    {
        base.BeginInteraction(tool);
        storedRad = triggerSphere.radius;
        triggerSphere.radius *= 3;
    }


    public override void EndInteraction()
    {
        base.EndInteraction();
        triggerSphere.radius = storedRad; 
    }



    //Override this in children for more specific check healths
    public virtual void CheckHealth() // All particleSystems references used here. Temprarily removed
    {
        if (stats.health >= 50 && stats.health < 100) // Active
        {
            if (state != State.Active)
            {
                if (state == State.Inactive)
                {
                    for (int i = 0; i < ghostTool.Count; i++) //Turns off any possible Ghost Tools (Tape/Cork)
                    {
                        ghostTool[i].SetActive(false);
                    }
                }
                else if (state == State.ActiveFull)
                {
                    waitingForRegen = false;
                }
                
                stats.dead = false;
                state = State.Active;
            }

            gm.roomStats.health -= (((stats.health / 100) * stats.attack) * Time.deltaTime);
        }
        if (stats.health < 50 && stats.health > 0) // Inactive
        {
            if (state != State.Inactive)
            {
                state = State.Inactive;
            }
        }
        
        if (stats.health >= 100) // Active Full
        {
            if (state != State.ActiveFull)
            {
                if (stats.health > 100)
                {
                    stats.health = 100;
                    waitingForRegen = true;
                }

                for (int i = 0; i < ghostTool.Count; i++) //Turns off any possible Ghost Tools (Tape/Cork)
                {
                    ghostTool[i].SetActive(false);
                }
                
                state = State.ActiveFull;
            }
            gm.roomStats.health -= (((stats.health / 100) * stats.attack) * Time.deltaTime);
        }
        if (stats.health <= 0) // Dead
        {
            if (waitingForRegen == false)
            {
                //particleSystems[2].gameObject.SetActive(false);
                waitingForRegen = true;
                StartCoroutine(Dead());
            }
        }
    }

    protected void EnableProperParticleSystems()
    {
        // Turn off all Particle Systems
        foreach (GameObject go in particleSystems)
        {
            go.SetActive(false);
        }

        if (state == State.Kill || state == State.Standby)
        {
            return;
        }

        int index = belowWater ? (int)state + 4 : (int)state;
        // Turn on whichever system we are currently using
        particleSystems[index].SetActive(true);
    }

    protected virtual void HandleHealthUI()
    {
        if (isInteracting)
        {
            if (healthUI.enabled == false)
                healthUI.enabled = true;

            healthUIIndex = (int)Mathf.Round(Mathf.Lerp(gm.LeakHealthUI.Length - 1, 0, stats.health / stats.maxHealth));
            healthUI.sprite = gm.LeakHealthUI[healthUIIndex];
        }
        else if (healthUI.enabled == true && !isInteracting)
        {
            healthUI.enabled = false;
        }
    }

    public virtual void Replace()
    {
        // To be overwritten by event 
    }
}
