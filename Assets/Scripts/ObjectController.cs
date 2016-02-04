using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
       Rigidbody rb = GetComponent<Rigidbody>();
       rb.AddForce(transform.forward * speed);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
