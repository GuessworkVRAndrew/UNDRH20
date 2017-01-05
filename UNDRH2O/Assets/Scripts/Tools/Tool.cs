//UNDR[H2O]
//Andrew Decker
//7/30/2016
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NewtonVR;

public class Tool : NVRInteractableItem {
    
    protected Stats myStats;
    public Stats MyStats { get { return myStats; } }
    //Used in Interacting with correct Tool Interactable
    [HideInInspector]
    public HashSet<ToolInteractable> TIsHoveringOver = new HashSet<ToolInteractable>();
    ToolInteractable closestTI;
    protected ToolInteractable interactingTI;
    public bool requiresButtonForUse;
    //Used for Indicating Interactability to the player
    MeshRenderer[] storedToolRenderers;
    public Material[] StoredToolMats { get { return storedToolMats; } }
    Material[] storedToolMats;
    public Material promptMat;

    protected GameObject myTool; //Model for tool,  MUST BE FIRST CHILD IN HEIRARCHY
    public GameObject MyTool { get { return myTool; } }
    public GameObject myPrefab;

    protected bool toolInUse;
    public bool ToolInUse { get { return toolInUse; } }



    protected override void Start()
    {
        base.Start();

        myStats = GetComponent<Stats>();

        myTool = transform.GetChild(0).gameObject;

        storedToolRenderers = GetComponentsInChildren<MeshRenderer>();
        storedToolMats = new Material[storedToolRenderers.Length];
        for (int i = 0; i < storedToolRenderers.Length; i++)
        {
            storedToolMats[i] = storedToolRenderers[i].material;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (TIsHoveringOver.Count != 0)
        {
            if (AttachedHand != null && requiresButtonForUse)
            {
                if (AttachedHand.UseButtonDown)
                    BeginInteractingWithClosest();
                else if (interactingTI && interactingTI.AttachedTool == this && (AttachedHand.UseButtonUp | AttachedHand.HoldButtonUp))
                    BreakConnection(); //Directly Calls to TI, no other interaction on tool
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        ToolInteractable colTI = col.GetComponent<ToolInteractable>();
        if (colTI)
        {
            TIsHoveringOver.Add(colTI);
            if (!toolInUse && IsCompatible(colTI) && !colTI.IsInteracting && !colTI.Stats.dead)
            {
                Prompt();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        ToolInteractable colTI = col.GetComponent<ToolInteractable>();
        if(colTI)
        {
            if (interactingTI && colTI == interactingTI && interactingTI.AttachedTool == this && interactingTI.IsInteracting) //leaving interactable zone while in interaction
            {
                Debug.Log("Trigger Exit from " + colTI.name + " while interacting");
                BreakConnection();
            }
            TIsHoveringOver.Remove(colTI);
            if (TIsHoveringOver.Count == 0 || AllTIsInactive())
                EndPrompt();
        }
    }

    public virtual bool IsCompatible(ToolInteractable colTI)
    {
        Debug.LogWarning("IsCompatible has not been overwritten in child class! Are you sure this tool should be compatible with EVERYTHINHG?");
        return true;
    }

    public virtual void BreakConnection()
    {
        myTool.SetActive(true);

        interactingTI.EndInteraction();
        interactingTI = null;

        if (AllTIsInactive())
            EndPrompt();
    }

    /*void FixedUpdate()
    {
        if (fixing == true)
            CheckInteraction(); //I have no idea if we need to implement this or not.. Don't delete it though
    }*/

    void BeginInteractingWithClosest()
    {
        float minDist = float.MaxValue;
        float distance;

        if (TIsHoveringOver != null)
        {
            HashSet<ToolInteractable> TIsStagedForRemoval = new HashSet<ToolInteractable>();
            foreach (Leak item in TIsHoveringOver)
            {
                if (item)
                {
                    distance = (item.transform.position - transform.position).sqrMagnitude;

                    if (distance < minDist)
                    {
                        minDist = distance;
                        closestTI = item;
                    }
                }
                else
                    TIsStagedForRemoval.Add(item);
            }
            foreach(Leak gone in TIsStagedForRemoval)
            {
                TIsHoveringOver.Remove(gone);
            }
        }

        interactingTI = closestTI;
        closestTI = null;
        
        CheckInteraction();
    }

    protected virtual void CheckInteraction()
    {
        //NOTE: This always must be overwritten with no base.CheckInteraction()
        //unless the tool is compatible with ALL Tool Interactables
        Debug.LogWarning("Omni Compatable Check interaction called, are you sure this tool should be compatable with EVERYTHING?");
        if (interactingTI.IsInteracting)
        {
            interactingTI.EndInteraction();
        }
        myTool.SetActive(false);
        interactingTI.BeginInteraction(this);
    }
    

    void Prompt()
    {
        //Debug.Log("Prompting user for Tool Interaction, " + name + " should be prompt colors.");
        for (int i = 0; i < storedToolRenderers.Length; i++)
        {
            storedToolRenderers[i].material = promptMat;
        }

    }

    bool AllTIsInactive()
    {
        if (TIsHoveringOver.Count != 0)
        {
            foreach (ToolInteractable TI in TIsHoveringOver)
            {
                if (TI.RecievingTools)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void EndPrompt()
    {
        for (int i = 0; i < storedToolRenderers.Length; i++)
        {
            storedToolRenderers[i].material = storedToolMats[i];
        }

    }
}
