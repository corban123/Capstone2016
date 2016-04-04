using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ElementMarkerScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        print (gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cam = Camera.main.transform.position;
        Vector3 go = gameObject.transform.position;
        Vector3 test = cam - go;
        gameObject.transform.forward = test;
	}
}
