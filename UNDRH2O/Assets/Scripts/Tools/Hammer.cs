using UnityEngine;
using System.Collections;

public class Hammer : Tool {


    protected override void Update()
    {
        base.Update();
    }

    

    protected override void CheckInteraction()
    {
        //Attempt to cast interactingTI as all compatible Classes
        //If the tool does not require a button for use, then it will be called from collision/Trigger enter instead of here
        HoleLeak holeLeak = interactingTI as HoleLeak;

        if(holeLeak /*|| otherCompatibleType*/) //Checks if any matched
        {
            if (interactingTI.IsInteracting)
                interactingTI.EndInteraction();
            myTool.SetActive(false);
            if (holeLeak)
                holeLeak.BeginInteraction(this);

        }
        
    }

    public override void BreakConnection()
    {
        base.BreakConnection();

        StartCoroutine(Vibrate());
    }

    IEnumerator Vibrate()
    {
        for (float i = 0f; i < 0.25f; i += Time.deltaTime)
        {
            AttachedHand.TriggerHapticPulse((ushort)200);
            yield return null;
        }
    }
}