using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryManager : MonoBehaviour {

	public UnityEvent[] storyEvents;
	public bool startWithScene;

	public void Start()
	{
		if (startWithScene)
			StartStoryEvent (0);
	}

	public void StartStoryEvent(int num)
	{
		storyEvents [num].Invoke ();
	}
}
