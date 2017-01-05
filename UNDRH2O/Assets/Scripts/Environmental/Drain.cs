using UnityEngine;
using System.Collections;

public class Drain : MonoBehaviour {

	GameManager gm;
	Stats myStats;
	Transform water;
	public bool draining = false;
	// Use this for initialization
	void Start () 
	{
		gm = FindObjectOfType<GameManager> ();
		myStats = GetComponent<Stats> ();
		water = GameObject.Find ("Water").transform;
		myStats.maxHealth = myStats.health;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (water && water.position.y > transform.position.y + 0.1f) {
			gm.roomStats.health += (((myStats.health / myStats.maxHealth) * myStats.attack) * Time.deltaTime);
			draining = true;
		}
		else 
		{
			draining = false;
		}
	}

	void OnTriggerStay(Collider col)
	{
		if (draining == true && (col.tag == "Tool" || col.tag == "Interactive Object")) 
		{
			if (col.transform.parent == null || col.transform.parent.name.Contains ("Controller") == false)
			{
				col.transform.position = Vector3.MoveTowards (col.transform.position, transform.position, (myStats.health / myStats.maxHealth) * (0.1f/Vector3.Distance(col.transform.position, transform.position)) * Time.deltaTime);
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (draining == true && col.collider.tag == "Tool" || col.collider.tag == "Interactive Object")
			myStats.health -= col.collider.GetComponent<Stats>().attack;
	}

	void OnCollisionExit(Collision col)
	{
		if (draining == true && col.collider.tag == "Tool" || col.collider.tag == "Interactive Object")
			myStats.health += 10;
	}
}
