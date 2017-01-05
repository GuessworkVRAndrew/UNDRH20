using UnityEngine;
using System.Collections;

public class AnimatorDestroy : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
	}
	
}
