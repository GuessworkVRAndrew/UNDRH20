using UnityEngine;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    GameManager gm;

    public bool autoFollow;

    public GameObject playerHead;

    public float movementSpeed;
    public float rotationSpeed;
	// Use this for initialization
	void Awake ()
    {
        gm = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(gm.IsGameOver || autoFollow)
        {
            transform.position = Vector3.Lerp(transform.position, playerHead.transform.position, 0.1f * movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerHead.transform.rotation, 0.1f * rotationSpeed * Time.deltaTime);
        }
	}
}
