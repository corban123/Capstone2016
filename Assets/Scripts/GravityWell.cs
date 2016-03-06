using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityWell : MonoBehaviour
{
	float range = 50f;
    float pullForce = 1000f;
    float startTime;
    public int duration;
	void Start()
	{
        startTime = Time.time;
	}
	
	void Update () 
	{
        if(Time.time - startTime > duration)
        {
            Destroy(this.gameObject);
        }
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Player"))
            {
                print(collider.gameObject.name + " is getting pulled");
                
                // calculate direction from target to me
                Vector3 forceDirection = transform.position - collider.transform.position;
                print("direction black hole" + forceDirection.normalized);
                // apply force on target towards me
                rb.AddForce(forceDirection.normalized * pullForce);
                print("adding force " + forceDirection.normalized * pullForce);
            }
        }
    }

    void OnDestroy()
    { 
        print("lol");
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            if (collider.CompareTag("Player"))
            {
                // apply force on target towards me
                print("Black hole wins!");
                Destroy(collider.gameObject); //Replace this with respawn
            }
        }
    }

}
