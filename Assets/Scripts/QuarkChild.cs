using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class QuarkChild : NetworkBehaviour {

	// Use this for initialization

	public GameObject quark;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	[Command]
	public void CmdSpawn(){
		GameObject instance;
		instance = (Instantiate (quark, this.transform.position, this.transform.rotation)) as GameObject;
		NetworkServer.Spawn (instance);
	}
}
