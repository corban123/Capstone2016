using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class QuarkChild : NetworkBehaviour {


	public GameObject quark;
	private GameObject spawnedObject;
	private QuarkOverlord overlord;
	private int numTimesAdded;
	void Start () {
		overlord = GameObject.Find ("GenerateBoard").GetComponent<QuarkOverlord> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!spawnedObject && numTimesAdded == 1) {
			overlord.addToEmpty (this.gameObject);	
			numTimesAdded = 0;
		} else if (spawnedObject && numTimesAdded == 0) {
			overlord.addToSpawned (this.gameObject);
			numTimesAdded = 1;
		}

	}
	[Command]
	public void CmdSpawn(){
		GameObject instance;
		instance = (Instantiate (quark, this.transform.position, this.transform.rotation)) as GameObject;
		spawnedObject = instance;
		NetworkServer.Spawn (instance);
	}
}
