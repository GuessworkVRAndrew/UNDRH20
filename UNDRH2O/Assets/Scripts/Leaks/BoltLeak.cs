using UnityEngine;
using System.Collections;

public class BoltLeak : Leak
{
    public Transform bolt;

    public Transform ghostWrenchTrans;
    GameObject flattenedWrenchPos;
    FixedJoint joint;
    bool boltWasFlipped;

    Vector3 euler;
    Vector3 eulerPrev;
    float xRot, xRotPrev;

	public AudioClip attatchClip, attatchClip2, DetatchClip;

    public override void Start()
    {
        base.Start();
        
        //ghostWrenchTrans = interactionObject.transform.Find("Ghost Wrench Transform");

        flattenedWrenchPos = new GameObject();
        flattenedWrenchPos.transform.SetParent(this.transform);
        flattenedWrenchPos.name = "Flattened Wrench Pos";
        flattenedWrenchPos.transform.position = Vector3.zero;
        attatchedToolType = ToolType.None;
    }

    public override void Update()
    {

        base.Update();

        if (attatchedToolType == ToolType.Wrench)
            CheckWrench();
    }

    public override void BeginInteraction(Tool tool)
    {
        base.BeginInteraction(tool);

        Wrench wrench = tool as Wrench;
        //Maybe Have a socket drill tool later that is a super tool?
        //Another Drill that is waterproof?

        if (wrench)
            BeginWrench(wrench);

        isInteracting = true;
    }

    void BeginWrench(Wrench wrench)
    {
        
        
        
        ghostTool.Add(Instantiate<GameObject>(attachedTool.myPrefab));
        ghostTool[0].SetActive(true);

        Renderer[] toolRenderers = ghostTool[0].GetComponentsInChildren<Renderer>();
        for(int i = 0; i < toolRenderers.Length; i++)
        {
            toolRenderers[i].material = wrench.StoredToolMats[i];
        }
        ghostTool[0].transform.SetParent(ghostWrenchTrans);
        ghostTool[0].transform.localPosition = ghostWrenchTrans.localPosition;
        ghostTool[0].transform.localRotation = Quaternion.identity;

        xRotPrev = interactionObject.transform.localRotation.x;

        // AUDIO HERE : Wrench attach noise
		source.PlayOneShot(attatchClip);
		source.PlayOneShot(attatchClip2);

        attatchedToolType = ToolType.Wrench;
    }

    void CheckWrench()
    {

        Vector3 wrenchRelative = this.transform.InverseTransformPoint(attachedTool.transform.position);
        flattenedWrenchPos.transform.localPosition = wrenchRelative;
        flattenedWrenchPos.transform.localPosition = new Vector3 (flattenedWrenchPos.transform.localPosition.x, flattenedWrenchPos.transform.localPosition.y, 0f);
        interactionObject.transform.LookAt(flattenedWrenchPos.transform);
        xRot = interactionObject.transform.localRotation.x;

        float xChange = xRot - xRotPrev;

        //Weird Directional Compensation
        //float compensate = (Mathf.Abs(transform.rotation.eulerAngles.y) % 360) < 125f ? -1 : 1;

        Debug.Log(interactionObject.transform.localRotation.eulerAngles.y);
        //Debug.Log(xChange);
        if (interactionObject.transform.localRotation.eulerAngles.y >= 180f) //Right side and wrench going righty tighty
        {
            //Debug.Log("Left");
            bolt.transform.localRotation = Quaternion.Euler(180f, 270f, 0f);
            if (xChange > 0f)
            {
                //Debug.Log("Woah!");
                stats.health -= attachedTool.MyStats.attack * Time.deltaTime;
            }
            else
            {
                //Debug.Log("No!");
                stats.health += attachedTool.MyStats.attack * Time.deltaTime;
                if (stats.health > stats.maxHealth)
                    stats.health = stats.maxHealth;
            }
        }
        else if (interactionObject.transform.localRotation.eulerAngles.y < 180) //left side and wrench going righty tighty
        {
            //Debug.Log("Right");
            bolt.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
            if (xChange > 0f)
            {
                stats.health -= attachedTool.MyStats.attack * Time.deltaTime;
            }
            else
            {
                stats.health += attachedTool.MyStats.attack * Time.deltaTime;
                if (stats.health > stats.maxHealth)
                    stats.health = stats.maxHealth;
            }
        }

        xRotPrev = xRot;
        //TODO: 
        //  - Have Coroutine running that makes haptic pulse dependent on yChange rate
        //  - Have proper breakaway function on dead? Did I really not make that?
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
        attatchedToolType = ToolType.None;
        source.PlayOneShot(DetatchClip);
        ghostTool.Clear();
    }

    public override void CheckHealth()
    {
        if (stats.health >= 50 && stats.health < 100) // Active
        {
            if (state != State.Active)
            {
                if (state == State.Inactive)
                {
                    for (int i = 0; i < ghostTool.Count; i++) //Turns off any possible Ghost Tools (Tape/Cork)
                    {
                        if (!keepGhostOnDeath)
                        {
                            //DestroyAllGhosts();
                        }
                    }
                }
                else if (state == State.ActiveFull)
                {
                    waitingForRegen = false;
                }

                stats.dead = false;

                state = State.Active;

                EnableProperParticleSystems();
            }

            gm.roomStats.health -= (((stats.health / 100) * stats.attack) * Time.deltaTime);
            
        }
        if (stats.health < 50 && stats.health > 0) // Inactive
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
                    waitingForRegen = true;
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
                if(attachedTool)
                    attachedTool.BreakConnection();
                EndInteraction();
                state = State.Dead;
                EnableProperParticleSystems();
                waitingForRegen = true;
                StartCoroutine(Dead());
            }
        }
    }


}
