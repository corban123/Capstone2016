using UnityEngine;
using System.Collections;


public class QuarkOverlord : MonoBehaviour {
	ArrayList preparedToSpawn;
	ArrayList spawnedList;
	ArrayList empty;
	private GameObject objects;

	// Use this for initialization
	void Start () {
		spawnedList = new ArrayList();
		preparedToSpawn = new ArrayList ();
		empty = new ArrayList ();
		if (objects = GameObject.Find ("QuarkMarkers")) {
			for (int i = 0; i < objects.transform.childCount - 10; i++) {
				QuarkChild child = objects.transform.GetChild (i).GetComponent<QuarkChild> ();
				child.numTimesAdded = 1;
				spawnedList.Add (child);	
	
			}
			for(int i = objects.transform.childCount-10; i < objects.transform.childCount; i++){
				QuarkChild child = objects.transform.GetChild (i).GetComponent<QuarkChild> ();
				child.preparedToSpawn = true;
				preparedToSpawn.Add(child);
			}
			spawnAll ();
		}
	}


	void spawnAll (){
		for(int i = 0; i < spawnedList.Count; i++){
			((spawnedList[i]) as QuarkChild).CmdSpawn ();
		}
	
	
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void addToEmpty(QuarkChild obj){
		empty.Add (obj);
	}

	public void addToSpawned(QuarkChild obj){
		spawnedList.Add (obj);
	
	}

	public void deSpawn(){
		GameObject toPreparedToSpawn = null;
		GameObject toSpawnedList = null;
		int killPreparedToSpawn = -1;
		int killToSpawnedList = -1;
		if (empty.Count > 0) {
			killPreparedToSpawn = Random.Range (0, empty.Count);
			toPreparedToSpawn = empty [killPreparedToSpawn] as GameObject;
			empty.RemoveAt (killPreparedToSpawn);

		}
		if (preparedToSpawn.Count > 0) {
			killToSpawnedList = Random.Range (0, preparedToSpawn.Count);

			toSpawnedList = preparedToSpawn [killToSpawnedList] as GameObject;
			(toSpawnedList.GetComponent<QuarkChild>()).CmdSpawn ();


			preparedToSpawn.RemoveAt (killToSpawnedList);
			spawnedList.Add (toSpawnedList);
			if (toPreparedToSpawn != null) {
				preparedToSpawn.Add (toPreparedToSpawn);
			}
		}
	

	}
}
