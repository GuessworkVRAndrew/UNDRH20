using UnityEngine;
using System.Collections;

public class ToolRespawn : MonoBehaviour {

    public GameObject wrenchPrefab, hammerPrefab, tapePrefab, corkPrefab;
	public Transform wrenchPrefabStartTrans, hammerPrefabStartTrans, tapePrefabStartTrans, corkPrefabStartTrans;

    Transform[] children;

    void Start()
    {
        children[0] = (Transform)Instantiate(wrenchPrefab, wrenchPrefabStartTrans.position, wrenchPrefabStartTrans.rotation, transform);
        children[1] = (Transform)Instantiate(hammerPrefab, hammerPrefabStartTrans.position, hammerPrefabStartTrans.rotation, transform);
        children[2] = (Transform)Instantiate(tapePrefab, tapePrefabStartTrans.position, tapePrefabStartTrans.rotation, transform);
        children[3] = (Transform)Instantiate(corkPrefab, corkPrefabStartTrans.position, corkPrefabStartTrans.rotation, transform);

        /*for(int i = 0; i < children.Length; i++)
        {
            children[i].GetComponent<>
        }
        */
        
    }
	// Update is called once per frame
	void Update ()
    {
        if(children[0].parent != transform)
            children[0] = (Transform)Instantiate(wrenchPrefab, wrenchPrefabStartTrans.position, wrenchPrefabStartTrans.rotation, transform);
        if (children[1].parent != transform)
            children[1] = (Transform)Instantiate(hammerPrefab, hammerPrefabStartTrans.position, hammerPrefabStartTrans.rotation, transform);
        if (children[2].parent != transform)
            children[2] = (Transform)Instantiate(tapePrefab, tapePrefabStartTrans.position, tapePrefabStartTrans.rotation, transform);
        if (children[3].parent != transform)
            children[3] = (Transform)Instantiate(corkPrefab, corkPrefabStartTrans.position, corkPrefabStartTrans.rotation, transform);
    }
}
