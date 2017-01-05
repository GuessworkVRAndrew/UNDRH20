using UnityEngine;
using System.Collections;

public class HammerablePlugGhost : MonoBehaviour {

    public HoleLeak myHole;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Hammer")
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb.velocity.magnitude > 2f)
            {
                myHole.HammerStrike((int)col.GetComponent<Stats>().attack);
            }
        }
    }
}
