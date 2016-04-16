using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PauseScript : NetworkBehaviour {
    readonly bool DEBUG = false;
    bool paused;
    FirstPersonController fpc;

	// Use this for initialization
	void Start () {
        paused = false;
        fpc = gameObject.GetComponent<FirstPersonController> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) {
            if (Input.GetKeyDown (KeyCode.Escape)) {
                paused = TogglePause ();
            }
        }
	}

    void onGUI() {
        if (paused) {
            print ("I'm paused!");
        }
    }

    bool TogglePause() {
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
            fpc.PauseFPC (false);

            return false;
        } else {
            fpc.PauseFPC (true);
            Time.timeScale = 0f;

            return true;
        }
    }
}
