using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class WaterFog : MonoBehaviour {

	GameManager gM;
	public bool under;
	public AudioMixerSnapshot normal;
	public AudioMixerSnapshot underWater;
	public Vector3 myPos;
	public Vector3 waterPos;

	// Use this for initialization
	void Start () 
	{
		gM = FindObjectOfType<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		myPos = transform.position;
		waterPos = gM.water.transform.position;
		if (transform.position.y < gM.water.position.y) 
		{
			if (under == false)
				gM.water.transform.localScale += new Vector3(0, -2, 0);
			under = true;
			RenderSettings.fog = true;
			underWater.TransitionTo (.1f);

		} 
		else 
		{
			if (under == true)
				gM.water.transform.localScale += new Vector3(0, 2, 0);
			under = false;
			RenderSettings.fog = false;
			normal.TransitionTo (.6f);

		}
	}
}
