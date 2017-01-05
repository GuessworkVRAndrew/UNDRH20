using UnityEngine;
using System.Collections;

public class AudioUtilities : MonoBehaviour {

	public static void AudioFlair(AudioSource source)
    {
        source.pitch = Random.Range(0.9f, 1.1f);
    }
}
