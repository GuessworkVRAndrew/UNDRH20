using UnityEngine;
using System.Collections;

public class Plug : Tool {


	protected override void Start()
    {
        base.Start();
    }

    public override bool IsCompatible(ToolInteractable colTI)
    {
        HoleLeak holeLeak = colTI as HoleLeak;

        if (holeLeak && !holeLeak.Corked && holeLeak.TimesTaped == 0)
            return true;

        return false;
    }

    protected override void CheckInteraction()
    {
        HoleLeak holeLeak = interactingTI.GetComponent<HoleLeak>();

        if(holeLeak)
        {
            if (interactingTI.IsInteracting)
                interactingTI.EndInteraction();
            myTool.SetActive(false);
            if (holeLeak)
                holeLeak.BeginInteraction(this);
        }
    }
}
