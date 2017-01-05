using UnityEngine;
using System.Collections;

public class Wrench : Tool {


    protected override void Update()
    {
        base.Update();
    }

    public override bool IsCompatible(ToolInteractable colTI)
    {
        BoltLeak boltLeak = colTI as BoltLeak;
        
        if (boltLeak && boltLeak.Stats.dead == false)
            return true;

        return false;
    }

    protected override void CheckInteraction()
    {
        //Attempt to cast interactingTI as all compatible Classes
        BoltLeak boltLeak = interactingTI as BoltLeak;

        if(IsCompatible(interactingTI)) //Checks if any matched
        {
            if (interactingTI.IsInteracting)
                interactingTI.EndInteraction();
            myTool.SetActive(false);
            boltLeak.BeginInteraction(this);

        }
        
    }

    public override void BreakConnection()
    {
        base.BreakConnection();

        //StartCoroutine(Vibrate(0.25f, 0.25f));
    }

    IEnumerator Vibrate(float length, float strength)
    {
        for (float i = 0f; i < length; i += Time.deltaTime)
        {
            AttachedHand.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }
}