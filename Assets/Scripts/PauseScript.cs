using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PauseScript : NetworkBehaviour {
    readonly bool DEBUG = false;
    public bool paused;
    FirstPersonController fpc;

	// Use this for initialization
	void Start () {
        paused = false;
        fpc = gameObject.GetComponent<FirstPersonController> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) {
            if (Input.GetKeyDown (KeyCode.Escape) && !DEBUG) {
                paused = TogglePause ();
            }
        }
	}

    void OnGUI() {
        if (paused && !DEBUG) {
            GUILayout.Label("Game is paused!");
            if(GUILayout.Button("Click me to unpause"))
                paused = TogglePause();
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
