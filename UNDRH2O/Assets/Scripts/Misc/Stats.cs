using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour 
{
//	GameManager gm;
	public float maxHealth;
	public float health;
	public float attack;
	public bool dead;

	void Start()
	{
		maxHealth = health;
//		gm = FindObjectOfType<GameManager> ();
	}
	void Update()
	{
		if (health <= 0 && dead == false)
		{
			health = 0;
			dead = true;
		}
		if (health > maxHealth)
			health = maxHealth;
	}
}
