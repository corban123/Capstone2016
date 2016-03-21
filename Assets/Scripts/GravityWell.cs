using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class GravityWell : NetworkBehaviour
{
	float range = 50f;
    float pullForce = 1000f;
    public int duration;
	void Start()
	{
        Destroy(this.gameObject, duration);
	}
	
	void Update () 
	{

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
                //rb.AddForce(forceDirection.normalized * pullForce);
                rb.AddExplosionForce(pullForce * -1, transform.position, 0);
                print("adding force " + forceDirection.normalized * pullForce);
                rb.gameObject.GetComponent<FirstPersonController>().moveFactor = 0;
            }
        }
    }

    void OnDestroy()
    {
        CmdRemoveBlackHole();
    }

    [Command]
    void CmdRemoveBlackHole()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<MoveScript>().Respawn();
                //Destroy(collider.gameObject);
                //NetworkServer.Destroy(collider.gameObject); //Replace this with respawn
            }
        }
        NetworkServer.Destroy(this.gameObject);
    }

    

}
