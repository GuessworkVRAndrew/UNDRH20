using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class StoryManager : MonoBehaviour {

	[System.Serializable]
	public class StoryEvent
	{
		[Range(0,10)]
		public float timeDelay;
		public float variance;
		public UnityEvent unityEvent;
	}
	public bool startWithScene;
	public StoryEvent[] storyEvents;
	public void Start()
	{
		if (startWithScene)
			StartStoryEvent (0);
	}
		
	public void StartStoryEvent(int num)
	{
		StartCoroutine (Wait(Random.Range(storyEvents [num].timeDelay - storyEvents [num].variance, storyEvents [num].timeDelay + storyEvents [num].variance)));
		storyEvents [num].unityEvent.Invoke ();
	}

	IEnumerator Wait(float num)
	{
		yield return new WaitForSeconds (num);
	}
}
