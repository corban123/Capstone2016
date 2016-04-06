using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnablerScript : NetworkBehaviour {
    private readonly int PLAYER1_LAYER = 8;
    private readonly int PLAYER2_LAYER = 9;

	CharacterController characterController;

	MoveScript move;
    FirstPersonController fpC;
    CombatScript combat;

	[SerializeField] Camera FPSCam;
	[SerializeField] AudioListener audioListen;
    int layerMask;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			// Enable the character controller for this player
			characterController = GetComponent<CharacterController> ();
			characterController.enabled = true;

			// Enable the move script for this player
			move = GetComponent<MoveScript> ();
			fpC = GetComponent<FirstPersonController> ();
			combat = GetComponent<CombatScript> ();
			move.enabled = true;
			fpC.enabled = true;
			characterController.enabled = true;
			combat.enabled = true;

			// Enable the camera and audio for this player
			FPSCam.enabled = true;

            if (gameObject.name == "Player 1") {
                layerMask = 1 << PLAYER1_LAYER;
            } else {
                layerMask = 1 << PLAYER2_LAYER;
            }
            FPSCam.cullingMask = ~layerMask;
			audioListen.enabled = true;
			foreach(Renderer r in GetComponentsInChildren<Renderer>()){
                if(r.gameObject.name != "pasted__tube"){
                    r.enabled = false;
                }
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
