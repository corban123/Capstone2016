using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MainMenuAudioScript : MonoBehaviour {
    public AudioClip startClip;
    public AudioClip loopClip;

    AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource> ();
        StartCoroutine(playEngineSound());
	}
    IEnumerator playEngineSound()
    {
        audio.clip = startClip;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = loopClip;
        audio.Play();
    }
	
}
