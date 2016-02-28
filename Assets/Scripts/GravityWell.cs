using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityWell : MonoBehaviour
{
	public float range = 1000;
	void Start()
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, range);
		foreach( Collider c in cols )
		{
            GameObject b = c.gameObject;
			if( b.CompareTag("Player" ))
            {
				Rigidbody body = c.gameObject.GetComponent<Rigidbody>();
				//Rigidbody body = component as Rigidbody;
				if( body != null )
                {
					body.useGravity = false;
					body.mass = 5;
					body.constraints |= RigidbodyConstraints.FreezePositionZ;
				}
			}
		}
	}
	
	void FixedUpdate () 
	{
		Collider[] cols  = Physics.OverlapSphere(transform.position, range); 
		List<Rigidbody> rbs = new List<Rigidbody>();
 
		foreach(Collider c in cols)
		{
			Rigidbody rb = c.attachedRigidbody;
			if(rb != null && rb != GetComponent<Rigidbody>() && !rbs.Contains(rb))
			{
				rbs.Add(rb);
				Vector3 offset = transform.position - c.transform.position;
				Vector3 force = offset / offset.sqrMagnitude * GetComponent<Rigidbody>().mass;
				if( force.magnitude > 500 ) force = force.normalized * 500;
				rb.AddForce( offset / offset.sqrMagnitude * GetComponent<Rigidbody>().mass);
			}
		}
	}
}
