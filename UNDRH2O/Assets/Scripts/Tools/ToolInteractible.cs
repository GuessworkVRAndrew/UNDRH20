using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolInteractable : MonoBehaviour {

    protected Stats stats;
    public Stats Stats { get { return stats; } }
    protected GameObject interactionObject;
    public GameObject InteractionObject { get { return interactionObject; } }
    protected List<GameObject> ghostTool;
    public List<GameObject> GhostTool { get { return ghostTool; } }
    public bool keepGhostOnDeath;
    protected bool isInteracting;
    public bool IsInteracting { get { return isInteracting; } }
    protected bool recievingTools;
    public bool RecievingTools { get { return recievingTools; } }
    protected Tool attachedTool;
    public Tool AttachedTool { get { return attachedTool; } }
    protected Collider triggerZone;
    protected ToolType attatchedToolType = ToolType.None;

    public enum ToolType
    {
        None,
        Wrench,
        Hammer,
        Plug,
        Tape,
        Drill
    }

    public virtual void Start()
    {
        stats = GetComponent<Stats>();
        interactionObject = transform.FindChild("Interaction Object").gameObject;
        triggerZone = GetComponent<Collider>();
        ghostTool = new List<GameObject>();
    }

    public virtual void Update()
    {

    }

    public virtual void EndInteraction()
    {
        if (!keepGhostOnDeath)
        {
            DestroyAllGhosts();
        }

        attachedTool = null;
        isInteracting = false;
        attatchedToolType = ToolType.None;
    }

    public virtual void BeginInteraction(Tool tool)
    {
        attachedTool = tool;
    }

    protected void DestroyAllGhosts()
    {
        GameObject temp;
        for (int i = ghostTool.Count - 1; i >= 0; i--) //Turn Ghost Tools off
        {
            temp = ghostTool[i];
            ghostTool.RemoveAt(i);
            Destroy(temp);
        }
    }
	
}
