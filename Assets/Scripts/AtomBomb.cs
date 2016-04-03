using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AtomBomb : NetworkBehaviour {

    float inner = 10f;
    float outer = 20f;
    float pushforce = 30f;
    public int duration;
    private Collider[] victims;
    void Start()
    {      
        Destroy(this.gameObject, duration);
        foreach (Collider collider in Physics.OverlapSphere(transform.position, inner))
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Player"))
            {
                print(rb.gameObject.name + " got caught in the explosion");
                rb.GetComponent<MoveScript>().Respawn();
            }
        }
        
    }

    void Update()
    {
        victims = Physics.OverlapSphere(transform.position, outer);
        foreach (Collider collider in victims)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Player"))
            {
                print(collider.gameObject.name + " is getting pushed");
                //rb.AddExplosionForce(pushforce, transform.position, 0);
                Vector3 forceDirection = transform.position - collider.transform.position;
                forceDirection.Normalize();
                forceDirection *= -pushforce;
                rb.AddForce(forceDirection.x, 0, forceDirection.z, ForceMode.VelocityChange);
                print("adding force " + pushforce);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        print("Leaving atomic zone");
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb.gameObject.CompareTag("Player"))
        {
            rb.GetComponent<Rigidbody>().isKinematic = true;
            rb.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnDestroy()
    {
        foreach (Collider collider in victims)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Player"))
            {
                rb.GetComponent<Rigidbody>().isKinematic = true;
                rb.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        CmdRemoveBomb();
    }

    [Command]
    void CmdRemoveBomb()
    {
        NetworkServer.Destroy(this.gameObject);
    }



}