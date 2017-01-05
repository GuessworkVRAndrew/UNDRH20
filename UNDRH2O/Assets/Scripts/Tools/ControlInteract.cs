//UNDR[H2O]
//James Gartland
//5/30/2016
using UnityEngine;
using System.Collections;
public class ControlInteract : MonoBehaviour 
{
    //Old Interaction Class torn apart to make the Tool class and derived classes of Tool
    /*
	//Private Variables
	//Steam Package Classes
	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;

	

	
	

	

	//Used in Duct Tape Detection
	Vector3 tapeOrigin;
	Animator leftAnim, RightAnim;
	bool fixingLeft;

	//Used in Patch Detection
	public float distOrigin, distCurrent;
	GameObject activePatchGhost;
	Animator patchAnim;

	
	public tool myToolType; //Tool Typing
	public Material promptMat; //Tool Highlighting for when in interaction area
	public bool rightSideOperated;
	public bool placedInLeftHand;

	public float dist;

	public enum tool
	{
		wrench,
		ductTape,
		patch,
		bucket,
	}
	
	

	
	void CheckInteraction()
	{
		switch (myToolType) {
		case tool.ductTape:
			CheckDuctInteraction ();
			break;
		case tool.patch:
			CheckPatchInteraction ();
			break;
		case tool.bucket:
			CheckBucketInteraction ();
			break;
		default:
			//Do nothing
			break;
		}
	}
	

	void CheckDuctInteraction()
	{
		if (fixing == false) 
		{
			fixing = true;
			tapeOrigin = transform.position; 
			if (hitLeak.leftPatched != true) 
			{
				Debug.Log ("Starting Left");
				hitLeak.ghostTool [0].SetActive (true);
				hitLeak.ghostTool [2].SetActive (true);
				leftAnim = hitLeak.ghostTool [2].GetComponent<Animator> ();
				leftAnim.SetBool ("Boom", true);
			}
			if (hitLeak.rightPatched != true && hitLeak.leftPatched) 
			{
				Debug.Log ("Starting Right");
				hitLeak.ghostTool [1].SetActive (true);
				hitLeak.ghostTool [3].SetActive (true);
				RightAnim = hitLeak.ghostTool [3].GetComponent<Animator> ();
			}
		}

		float dist = Vector3.Distance (tapeOrigin, transform.position);
		float step = dist / 0.4f;
		if (hitLeak.leftPatched !=  true && tapeOrigin.y - transform.position.y > 0) {
			//Debug.Log ("Fixing from Right to Left. Distance: " + dist);
			hitLeak.ghostTool [2].SetActive (true);
			hitLeak.ghostTool [3].SetActive (false);
			fixingLeft = true;
			leftAnim.SetFloat ("Distance", step);
		} 
		else if (hitLeak.rightPatched != true && hitLeak.leftPatched  && tapeOrigin.y - transform.position.y > 0) { //Top right to bottom left
			//Debug.Log ("Fixing from left to right. Distance: " + dist);
			hitLeak.ghostTool [2].SetActive (false);
			hitLeak.ghostTool [3].SetActive (true);
			fixingLeft = false;
			RightAnim.SetFloat ("Distance", step);
		}
		else 
		{
			//Debug.Log ("Distance," + dist);
		}
		if (dist >= .4f) 
		{
			hitLeak.leftPatched = true;
			hitLeak.ghostTool [0].SetActive (false);
			hitLeak.ghostTool [1].SetActive (false);
			hitLeak.ghostTool [2].SetActive (false);
			hitLeak.ghostTool [3].SetActive (false);
			hitLeak.ghostTool [4].SetActive (true);
			if(fixingLeft)
				hitLeak.ghostTool[5].SetActive(false);
			else
				hitLeak.ghostTool [5].SetActive (true);
			hitLeak.ghostTool [6].SetActive (false);
			hitLeak.ghostTool [7].SetActive (false);
			//hitStats.health -= myStats.attack;
			Debug.Log ("Breaking at 4");
			myTool.SetActive (true);
			StartCoroutine (BreakConnection());
		}
	}

	void CheckPatchInteraction()
	{
		if (fixing == false) 
		{
			fixing = true;
			Transform[] children = myTool.GetComponentsInChildren<Transform> ();
			for (int i = 0; i < children.Length; i++) 
			{
				if (children [i].gameObject.name.Contains ("Gum"))
					activePatchGhost = hitLeak.ghostTool [7];
					//Fix permaleak and return
				else if (children [i].gameObject.name.Contains ("Cork"))
					activePatchGhost = hitLeak.ghostTool [6];
			}
			activePatchGhost.SetActive (true);
			patchAnim = activePatchGhost.GetComponent<Animator> ();
			patchAnim.enabled = true;
			distOrigin = Vector3.Distance (hitLeak.transform.position, transform.position);
		}
		distCurrent = Vector3.Distance (hitLeak.transform.position, transform.position);
		float step = 0.2f / (distCurrent / distOrigin);
		Debug.Log (step);
		patchAnim.SetFloat ("Plug", step);
		if (step >= 1f) 
		{
			hitLeak.ghostTool [0].SetActive (false);
			hitLeak.ghostTool [1].SetActive (false);
			hitLeak.ghostTool [2].SetActive (false);
			hitLeak.ghostTool [3].SetActive (false);
			if (activePatchGhost.name.Contains ("Cork")) 
			{
				hitLeak.ghostTool [6].SetActive (true);
				hitLeak.ghostTool [7].SetActive (false);
			}
			else if (activePatchGhost.name.Contains ("Gum")) 
			{
				hitLeak.ghostTool [6].SetActive (false);
				hitLeak.ghostTool [7].SetActive (true);
			}
			//hitStats.health -= myStats.attack;
			Debug.Log ("Breaking at 5");
			myTool = hitLeak.heldTool;
			StartCoroutine (BreakConnection());
		}
	}

	void CheckBucketInteraction()
	{
		Debug.Log ("BI");
	}*/
}