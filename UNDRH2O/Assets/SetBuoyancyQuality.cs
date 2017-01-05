using UnityEngine;
using System.Collections;
using LostPolygon.DynamicWaterSystem;

public class SetBuoyancyQuality : MonoBehaviour {

	BuoyancyForce[] bf;

	// Use this for initialization
	void Start ()
	{
		bf = FindObjectsOfType<BuoyancyForce> ();
		foreach (BuoyancyForce b in bf) 
		{
			b.Resolution = 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
