using UnityEngine;
using System.Collections;

public class HoleLeak : Leak
{
    [Header("Plug")]
	//Plug Variables
    Transform cork;
    Transform unpluggedPos;
    Transform fullyPluggedPos;
    [HideInInspector]
    public GameObject popPlugPrefab;
    float plugStartingDistance;
    PlugType plugType;
    bool corked = false;
    bool corkInProgress = false;
    public bool Corked { get { return corked; } }
    public AudioClip corkClip;

    [Header("Tape")]
    //Tape Variables
    int timesTaped = 0;
    public int TimesTaped { get { return timesTaped; } }
    int maxTapeAmount = 3;
    public int MaxTapeAmount { get { return maxTapeAmount; } }
    bool tapeinProgress;
    [HideInInspector]
    public GameObject popTapePrefab;
    public Transform tapePositioner;
    Transform tapeStartPos;
    Vector3 tapeOrigin;
    Animator tapeAnim;
    GameObject tapeGO;
    public AudioClip tapeClip;

    [Header("Hammer")]
    //Hammer Variables
    public float hammerUIDuration = 1.0f;
    float preHammerHealth;
    float postHammerHealth;
    int hammerUIIndex;
    float hammerHealthUIStartTime;
    bool playHammerUI;

    public SpriteRenderer holeUI;

    public enum PlugType
    {
        None,
        Cork,
        Gum,
        Tape
    }

	

    public override void Start()
    {
        base.Start();

        unpluggedPos = interactionObject.transform.FindChild("Unplugged Position");
        fullyPluggedPos = interactionObject.transform.FindChild("Fully Plugged Position");
        tapeStartPos = interactionObject.transform.FindChild("Tape Starting Position");
        attatchedToolType = ToolType.None;
        plugType = PlugType.None;
        corked = false;
    }

    public override void Update()
    {
        base.Update();
        if (attatchedToolType == ToolType.Plug)
        {
            if (plugType == PlugType.Cork)
                CheckCork();

        }
        else if (attatchedToolType == ToolType.Tape)
        {
            CheckTape();
        }

        if (corked)
            cork.position = Vector3.Lerp(fullyPluggedPos.position, unpluggedPos.position, (stats.health / stats.maxHealth));
        
    }

    public override void Burst()
    {
        base.Burst();

        if (!keepGhostOnDeath)
        {
            DestroyAllGhosts();
        }

        if (corked && !corkInProgress)
        {
            Destroy(cork.gameObject);
            ghostTool.RemoveAt(0);
            corked = false;
            Instantiate(popPlugPrefab, transform.position, Quaternion.Euler(transform.forward));
        }

        if (timesTaped > 0)
        {
            Destroy(ghostTool[ghostTool.Count-1]);
            ghostTool.RemoveAt(ghostTool.Count-1);
            timesTaped = 0;
            Instantiate(popTapePrefab, transform.position, Quaternion.Euler(transform.forward));
        }

        holeUI.enabled = true;
    }

    public override void BeginInteraction(Tool tool)
    {
        base.BeginInteraction(tool);

        //Any tools that are not marked as requiresButtonForUse
        Plug plug = tool as Plug;

        Tape tape = tool as Tape;

        if (plug)
        {
            BeginPlug(plug);
        }
        if(tape)
        {
            BeginTape(tape);
        }
        

        isInteracting = true;
    }

    void BeginPlug(Plug plug)
    {
        ghostTool.Add((GameObject)Instantiate(attachedTool.myPrefab, new Vector3(0f, 0f, 0f), unpluggedPos.rotation, interactionObject.transform));
        ghostTool[0].SetActive(true);   // TODO: Find safer way to access this ghost
        cork = ghostTool[0].transform;
        //cork.SetParent(interactionObject.transform);
        //cork.position = new Vector3(0f, 0f, 0f);
        //cork.rotation = Quaternion.Euler(90f, 0f, 0f);
        cork.GetComponent<HammerablePlugGhost>().myHole = this;
        plugStartingDistance = Vector3.Distance(attachedTool.transform.position, transform.position);

        //Used for checking if we need to deleted the cork when the interaction ends
        corkInProgress = true;

        source.PlayOneShot(corkClip);

        attatchedToolType = ToolType.Plug;
        if (attachedTool.name == "Cork")
        {
            plugType = PlugType.Cork;
            corked = true;
        }
    }

    void CheckCork()
    {
        float plugCurrentDistance = Vector3.Distance(attachedTool.transform.position, transform.position);
        float normalizedDist = Mathf.Clamp((plugCurrentDistance / plugStartingDistance) -.5f, 0f, 1f); //Adjusts the plug to move in only to where it is logically finger tight
        //lerp health based on hand distance
        stats.health = Mathf.Lerp(50, stats.maxHealth, normalizedDist);

        if (stats.health <= 50f)
        {
            //LEts the script know that we are down corking the pipe
            corkInProgress = false;
        }
    }

    public void HammerStrike(int damage)
    {
        // Set up variables for Hammer UI
        if (!playHammerUI)
        {
            playHammerUI = true;
            preHammerHealth = stats.health;
            hammerHealthUIStartTime = Time.time;
        }
        stats.health -= damage;
        postHammerHealth = stats.health;
    }

    void BeginTape(Tape tape)
    {
        AudioUtilities.AudioFlair(source);
        source.PlayOneShot(tapeClip);
        tapePositioner.localRotation = Quaternion.Euler(0, 0, 45 * timesTaped);
        tapeGO = (GameObject)Instantiate(attachedTool.myPrefab, tapeStartPos.position, tapeStartPos.rotation, interactionObject.transform);
        tapeGO.SetActive(true);
        tapeAnim = tapeGO.GetComponent<Animator>();
        
        tapeOrigin = attachedTool.transform.position;
        attatchedToolType = ToolType.Tape;
        isInteracting = true;
    }

    void CheckTape()
    {
        float dist = Vector3.Distance(tapeOrigin, attachedTool.transform.position);
        
        float step = dist / 0.4f;
        tapeAnim.SetFloat("Distance", step);

        if(step >= 1f)
        {
            stats.health -= attachedTool.MyStats.attack;

            Tape tape = attachedTool as Tape;
            Quaternion rot;
            if (timesTaped == 0)
                rot = Quaternion.Euler(new Vector3(83, 47, 61));
            else if (timesTaped == 1)
                rot = Quaternion.Euler(new Vector3(157, -99, -83));
            else
                rot = Quaternion.Euler(new Vector3(95, -2, 9));


            ghostTool.Add((GameObject)Instantiate(
                tape.levelsOfTape[timesTaped],
                interactionObject.transform.position,
                Quaternion.identity,
                transform));
            ghostTool[ghostTool.Count - 1].transform.localRotation = rot;

            Destroy(tapeGO);

            timesTaped++;

            attachedTool.BreakConnection();
            EndInteraction();
        }

    }

    public override void EndInteraction()
    {
        base.EndInteraction();

        if (tapeGO)
            Destroy(tapeGO);
        if (cork && corkInProgress)
        {
            Destroy(cork.gameObject);
            ghostTool.RemoveAt(0);
            corked = false;
        }
        attatchedToolType = ToolType.None;
    }
    

    public override void CheckHealth()
    {
        if (stats.health > 50 && stats.health < 100) // Active
        {
            if (state != State.Active)
            {
                //if (state == State.Inactive)
                //{
                    //for (int i = 0; i < ghostTool.Count; i++) //Turns off any possible Ghost Tools (Tape/Cork)
                    //{
                    //    ghostTool[i].SetActive(false);
                    //}
                //}
                /*else */if (state == State.ActiveFull)
                {
                    waitingForRegen = false;
                }

                

                stats.dead = false;
                state = State.Active;
                EnableProperParticleSystems();
            }

            gm.roomStats.health -= (((stats.health / 100) * stats.attack) * Time.deltaTime);
            
        }
        else if (stats.health == 50f)
        {
            if(state == State.Active && attatchedToolType == ToolType.Plug) //Remove Tool and make cork hammerable
            {
                Destroy(attachedTool.gameObject);
                EndInteraction();
            }
        }
        else if (stats.health < 50 && stats.health > 0) // Inactive
        {
            if (state != State.Inactive)
            {
                state = State.Inactive;

                EnableProperParticleSystems();
            }
        }

        if (stats.health >= 100) // Active Full
        {
            if (state != State.ActiveFull)
            {
                if (stats.health > 100)
                {
                    stats.health = 100;
                    waitingForRegen = true; // Why regenerate health when we don't need to?
                }
                if (ghostTool != null)
                {
                    if (respawns)
                        Burst();
                    //TODO:
                    //  - Shoot off gameobject at random preferably outward velocity
                    //  - Make a small water particle explosion
                }

                state = State.ActiveFull;

                EnableProperParticleSystems();

            }
            gm.roomStats.health -= (((stats.health / 100) * stats.attack) * Time.deltaTime);
        }
        if (stats.health <= 0) // Dead
        {
            if (waitingForRegen == false)
            {
                if(attatchedToolType == ToolType.Tape)
                    attachedTool.BreakConnection();
                EndInteraction();
                state = State.Dead;
                EnableProperParticleSystems();
                waitingForRegen = true;
                StartCoroutine(Dead());
            }
        }

        //If we can regenerate, do so!
        if(!waitingForRegen)
        {
            stats.health += regenRate * Time.deltaTime;
        }
    }

    protected override void HandleHealthUI()
    {

        if (playHammerUI)
        {
            if (healthUI.enabled == false)
                healthUI.enabled = true;

            int healthUIStartIndex = (int)Mathf.Round(Mathf.Lerp(gm.LeakHealthUI.Length - 1, 0, preHammerHealth / stats.maxHealth));
            int healthUITargetIndex = (int)Mathf.Round(Mathf.Lerp(gm.LeakHealthUI.Length - 1, 0, postHammerHealth / stats.maxHealth));

            //float progress = (Time.time - hammerHealthUIStartTime) * hammerUIDuration;
            //float fracJourney = progress / hammerHealthUIDifference;
            healthUI.sprite = gm.LeakHealthUI[(int)Mathf.Round(Mathf.Lerp(healthUIStartIndex, healthUITargetIndex, hammerUIDuration * Time.deltaTime))];

            //hammerHealthUIStartTime += Time.deltaTime;
            if (Time.time >= hammerHealthUIStartTime + hammerUIDuration)
            {
                playHammerUI = false;
                healthUI.enabled = true;
            }
        }
        else
        {
            base.HandleHealthUI();
        }
    }
}
