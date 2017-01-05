using UnityEngine;
using System.Collections;
using NewtonVR;

public class TVRemote : MonoBehaviour {

    NVRInteractableItem item;

    public Material[] TVMats;
    public Material TVStatic;
    public MeshRenderer TVRenderer;

    int channel = 0;

    void Awake()
    {
        item = GetComponent<NVRInteractableItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (item.AttachedHand && item.AttachedHand.UseButtonDown)
        {
            StartCoroutine(ChangeChannel());
        }
    }

    IEnumerator ChangeChannel()
    {
        TVRenderer.material = TVStatic;
        yield return new WaitForSeconds(0.1f);
        channel = (channel + 1) % TVMats.Length;
        TVRenderer.material = TVMats[channel];
    }
}
