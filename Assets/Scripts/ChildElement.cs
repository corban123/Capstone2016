using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ChildElement : NetworkBehaviour {

	public GameObject element;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	[Command]
	public void CmdSpawn(){
		GameObject instance;
		instance = (Instantiate (element, new Vector3(this.transform.position.x, this.transform.position.y+2, this.transform.position.z), this.transform.rotation)) as GameObject;

		NetworkServer.Spawn (instance);
	}
}
