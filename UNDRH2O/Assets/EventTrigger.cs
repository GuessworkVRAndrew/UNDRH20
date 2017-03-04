using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour {

	public UnityEvent triggerEvent;
	//checks for string to avoid too many tags
	public string requiredName;

	void OnTriggerEnter (Collider hit) 
	{
		if (requiredName != null)
		{
			if (hit.gameObject.name != requiredName)
				return;
		}

		triggerEvent.Invoke ();
	}
}
