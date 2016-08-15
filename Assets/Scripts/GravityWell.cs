using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class GravityWell : NetworkBehaviour
{
	float range = 50f;
    float pullForce = 3000f;
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
                rb.AddExplosionForce(pullForce * -1, transform.position, 0);
                print("adding force " + forceDirection.normalized * pullForce);
                rb.gameObject.GetComponent<FirstPersonController>().moveFactor = 0;
            }
        }
    }

    void OnDestroy()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            if (collider.CompareTag("Player"))
            {                
                collider.GetComponent<MoveScript>().Respawn();
                collider.GetComponent<MoveScript>().showRespawn();
            }
        }
        CmdRemoveBlackHole();
    }

    [Command]
    void CmdRemoveBlackHole()
    {     
        NetworkServer.Destroy(this.gameObject);
    }

    

}
