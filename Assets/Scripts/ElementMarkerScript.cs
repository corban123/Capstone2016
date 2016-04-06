using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ElementMarkerScript : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (Camera.main)
        {
            Vector3 cam = Camera.main.transform.position;
            Vector3 go = gameObject.transform.position;
            Vector3 test = cam - go;
            gameObject.transform.forward = test;
        }
    }
}
