using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class QuarkChild : NetworkBehaviour {


	public GameObject quark;
	[SyncVar] public GameObject spawnedObject;
	private QuarkOverlord overlord;
	public bool preparedToSpawn;
	public int numTimesAdded;
	void Start () {
		numTimesAdded = 0;
		preparedToSpawn = false;
		overlord = GameObject.Find ("GenerateBoard").GetComponent<QuarkOverlord> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!spawnedObject && numTimesAdded == 1) {
			overlord.addToEmpty (this.gameObject.GetComponent<QuarkChild>());	
			numTimesAdded = 0;
		} else if (spawnedObject && numTimesAdded == 0) {
			overlord.addToSpawned (this.gameObject.GetComponent<QuarkChild>());
			numTimesAdded = 1;
		}

	}
	[Command]
	public void CmdSpawn(){
		GameObject instance;
		instance = (Instantiate (quark, new Vector3(this.transform.position.x, this.transform.position.y+2, this.transform.position.z), this.transform.rotation)) as GameObject;
		spawnedObject = instance;

		NetworkServer.Spawn (instance);
        checkForQuark();
	}

    public void checkForQuark()
    {
        Collider[] colliders;
        if ((colliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), 1f)).Length > 1)
        {
            preparedToSpawn = colliders[0];
        }
    }
}
