using UnityEngine;
using System.Collections;

public class Tape : Tool {

    public GameObject[] levelsOfTape;

    public override bool IsCompatible(ToolInteractable colTI)
    {
        HoleLeak holeLeak = colTI as HoleLeak;

        if (holeLeak && holeLeak.Stats.dead == false && !holeLeak.Corked && holeLeak.TimesTaped < holeLeak.MaxTapeAmount)
            return true;

        return false;
    }

    protected override void CheckInteraction()
    {
        HoleLeak holeLeak = interactingTI.GetComponent<HoleLeak>();

        if (IsCompatible(interactingTI))
        {
            if (interactingTI.IsInteracting)
                interactingTI.EndInteraction();
            myTool.SetActive(false);
            holeLeak.BeginInteraction(this);
        }
    }

}
