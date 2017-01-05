using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour 
{
	AudioSource source;
    public AudioClip switchClip;
	public GameObject[] lights;

	void Start()
	{
		source = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter()
	{
		source.PlayOneShot(switchClip);
		foreach (GameObject l in lights)
			l.SetActive (!l.activeSelf);
	}
}
