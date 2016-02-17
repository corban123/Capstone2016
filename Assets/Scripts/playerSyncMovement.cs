using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerSyncMovement : NetworkBehaviour {

	[SyncVar]
	private Vector3 syncPos;

	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		TransmitPosition ();
		LerpPosition ();

	}

	void LerpPosition()
	{
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.fixedDeltaTime * 30);
		}
	
	
	}


	[Command]
	void CmdProvidePositionToServer(Vector3 pos){
			syncPos = pos;	
	}

	[ClientCallback]
	void TransmitPosition(){
		if (isLocalPlayer) {
			CmdProvidePositionToServer (myTransform.position);
		}
	}

}
