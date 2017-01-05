using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PickupParent : MonoBehaviour 
{
    /*#region Variables
	public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
	public bool isLeftHand;

    //Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
    SteamVR_TrackedObject trackedObj;

    HashSet<InteractableItem> ObjectsHoveringOver = new HashSet<InteractableItem>();

    InteractableItem closestItem;
    InteractableItem interactingItem;
    #endregion

    //Grabs the Tracked Object Script at the beginning of the scene
    void Awake () 
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
  	}

	void Update () 
	{
        if (controller == null)
            Debug.Log("Controller not initialized");

        if (controller.GetPressDown(grip))
        {
            float minDist = float.MaxValue;
            float distance;

            foreach (InteractableItem item in ObjectsHoveringOver)
            {
                distance = (item.transform.position - transform.position).sqrMagnitude;

                if (distance < minDist)
                {
                    minDist = distance;
                    closestItem = item;
                }
            }

            interactingItem = closestItem;
            closestItem = null;

            if (interactingItem)
            {
                if(interactingItem.isInteracting())
                {
                    interactingItem.EndInteraction();
                }
                interactingItem.BeginInteraction(this);
            }
        }
            

        if (controller.GetPressUp(grip) && interactingItem)
        {
            interactingItem.EndInteraction();
        }
	}

    void OnTriggerEnter(Collider col)
    {
        InteractableItem collidedItem = col.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            Debug.Log("Triggered With " + col.name);
            ObjectsHoveringOver.Add(collidedItem);
        }
    }

    void OnTriggerExit (Collider col)
    {
        InteractableItem collidedItem = col.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            Debug.Log("Exiting Collision With " + col.name);
            ObjectsHoveringOver.Add(collidedItem);
        }
    }
    */
}