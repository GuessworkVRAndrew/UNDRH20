using UnityEngine;
using System.Collections;

public class HammerAudio : MonoBehaviour {

    AudioSource source;
    Rigidbody rb;
    public AudioClip hammerHit1, hammerHit2;
    float hammerTime;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

	void OnCollisionEnter(Collision col)
    {
        if(hammerTime < Time.timeSinceLevelLoad && rb.velocity.magnitude >= 0.5f)
        {
            hammerTime = Time.timeSinceLevelLoad + 0.25f;
            AudioUtilities.AudioFlair(source);
            source.PlayOneShot(hammerHit1);
            AudioUtilities.AudioFlair(source);
            source.PlayOneShot(hammerHit2);
        }
    }
}
