using UnityEngine;
using System.Collections;


public class QuarkOverlord : MonoBehaviour {
	ArrayList quarkList;
	ArrayList backupList;
	private GameObject objects;

	// Use this for initialization
	void Start () {
		quarkList = new ArrayList();
		backupList = new ArrayList ();
		if (objects = GameObject.Find ("QuarkMarkers")) {
			for (int i = 0; i < objects.transform.childCount - 10; i++) {
				quarkList.Add (objects.transform.GetChild (i).GetComponent<QuarkChild>());	
			
			}
			for(int i = objects.transform.childCount-10; i < objects.transform.childCount; i++){
				backupList.Add (objects.transform.GetChild (i).GetComponent<QuarkChild> ());
			}
			spawnAll ();
		}
	}


	void spawnAll (){
		for(int i = 0; i < quarkList.Count; i++){
			((quarkList[i]) as QuarkChild).CmdSpawn ();
		
		}
	
	
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void addToEmpty(GameObject obj){
	
	}

	public void addToSpawned(GameObject obj){
	
	
	}

	public void deSpawn(){
		int rand = Random.Range (0, backupList.Count-1);
		(backupList [rand] as QuarkChild).CmdSpawn();
		int rand2 = Random.Range (0, quarkList.Count - 1);
		GameObject tempObj = quarkList[rand2] as GameObject;
		quarkList.Remove (quarkList [rand2]);
		quarkList.Add (backupList [rand]);
		backupList.RemoveAt (rand);
		backupList.Add (tempObj);
	
	}
}
