using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PauseScript : NetworkBehaviour {
    readonly bool DEBUG = false;
    public bool paused;

    FirstPersonController fpc;
    GUIScript gui;
    Button unPauseButton;
    Button selfDestructButton;
    Button disconnectButton;

    Canvas pauseCanvas;

	// Use this for initialization
	void Start () {
        pauseCanvas = GameObject.Find ("PauseCanvas").GetComponent<Canvas>();
        unPauseButton = GameObject.Find("UnPauseButton").GetComponent<Button>();
        selfDestructButton = GameObject.Find("SelfDestructButton").GetComponent<Button>();
        disconnectButton = GameObject.Find("DisconnectButton").GetComponent<Button>();

        unPauseButton.onClick.AddListener ( () => { UnPauseButtonOnClick(); } );
        selfDestructButton.onClick.AddListener ( () => { SelfDestructButtonOnClick(); } );
        disconnectButton.onClick.AddListener ( () => { DisconnectButtonOnClick(); } );

        pauseCanvas.enabled = false;
        paused = false;
        fpc = gameObject.GetComponent<FirstPersonController> ();
        gui = gameObject.GetComponent<GUIScript> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) {
            if (Input.GetKeyDown (KeyCode.Escape) && !DEBUG) {
                TogglePause ();
            }
        }
	}

    void TogglePause() {
        if (Time.timeScale == 0f) {
            paused = false;
            Time.timeScale = 1f;
            fpc.PauseFPC (false);
            gui.enableCanvas ();
            pauseCanvas.enabled = false;
        } else {
            paused = true;
            fpc.PauseFPC (true);
            Time.timeScale = 0f;
            gui.disableCanvas ();
            pauseCanvas.enabled = true;
        }
    }

    public void UnPauseButtonOnClick() {
        if (isLocalPlayer) {
            TogglePause ();
        }
    }

    public void SelfDestructButtonOnClick() {
        if (isLocalPlayer) {
            MoveScript move = gameObject.GetComponent<MoveScript> ();
            move.Respawn ();
            TogglePause ();
        }
    }

    public void DisconnectButtonOnClick() {
        if (isLocalPlayer) {
            print ("disconnecting player");
        }
    }
}
