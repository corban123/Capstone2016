using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PauseScript : NetworkBehaviour {
    public bool TURN_PAUSE_MENU_OFF = false;
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
            if (Input.GetKeyDown (KeyCode.Escape)) {
                TogglePause ();
            }
        }
	}

    void TogglePause() {
        if (!TURN_PAUSE_MENU_OFF) {
            if (paused) {
                fpc.PauseFPC (false);
                gui.enableCanvas ();
                pauseCanvas.enabled = false;
            } else {
                fpc.PauseFPC (true);
                gui.disableCanvas ();
                pauseCanvas.enabled = true;
            }
            paused = !paused;
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
