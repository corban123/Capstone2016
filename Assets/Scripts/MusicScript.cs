using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicScript : MonoBehaviour {
    public AudioClip startClipMusic;
    public AudioClip loopClipMusic;
    public AudioSource musicSource;

    public AudioClip startClipVacuum;
    public AudioClip loopClipVacuum;
    public AudioSource vacuumSource;

    // Use this for initialization
    void Start () {
        StartCoroutine(playMusic());
    }

    public void startGameSounds() {
        StartCoroutine(playVacuum());
    }

    IEnumerator playMusic()
    {
        musicSource.clip = startClipMusic;
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = loopClipMusic;
        musicSource.Play();
    }


    IEnumerator playVacuum()
    {
        vacuumSource.clip = startClipVacuum;
        vacuumSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        vacuumSource.clip = loopClipVacuum;
        vacuumSource.Play();
    }
}
