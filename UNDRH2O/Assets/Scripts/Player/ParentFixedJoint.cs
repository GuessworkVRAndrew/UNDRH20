using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]

public class ParentFixedJoint : MonoBehaviour {

	//Steam Package Classes
	SteamVR_TrackedObject trackedObj;
	public SteamVR_Controller.Device device;
	Rigidbody rigidBodyAttachPoint;

	//Stores the Hinge created upon grabbing object
	FixedJoint fixedJoint;


	public Vector3 toolLockPosition;
	//public Transform sphere;

	void Awake () 
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		rigidBodyAttachPoint = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () 
	{
		device = SteamVR_Controller.Input ((int)trackedObj.index);
	}

	void OnTriggerStay (Collider col) 
	{
		if (col.gameObject.isStatic == false) 
		{
			if (fixedJoint == null && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) 
			{
				if (col.GetComponent<FixedJoint> () != null) //Checks if the other hand is holding the object and detaches it
				{ 
					//Object.Destroy (col.GetComponent<FixedJoint> ());
				}
				if (col.tag == "Tool") 
				{
					col.transform.position = transform.position + toolLockPosition;
					col.transform.rotation = transform.rotation;
				}
				//Attaches to hand
				fixedJoint = col.gameObject.AddComponent<FixedJoint> ();
				fixedJoint.connectedBody = rigidBodyAttachPoint;
			}
			else if (fixedJoint != null && device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) //Releases object
			{ 
				GameObject go = fixedJoint.gameObject;
				Rigidbody rigidBody = go.GetComponent<Rigidbody> ();
				fixedJoint = null;
				//Object.Destroy (fixedJoint);
				tossObject (rigidBody);
			}
		}
	}

	void tossObject(Rigidbody rigidBody) //Continues object momentum after release
	{
		Debug.Log ("Tossing Object");
		Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
		if (origin != null) {

			rigidBody.velocity = origin.TransformVector (device.velocity);
			rigidBody.angularVelocity = origin.TransformVector (device.angularVelocity);

		} else {

			GetComponent<Rigidbody>().velocity = device.velocity;
			rigidBody.angularVelocity = device.angularVelocity;
		}
}
}