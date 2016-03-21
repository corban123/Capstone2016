using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AtomBomb : NetworkBehaviour {

    float range = 1000f;
    float pushforce = 10000f;
    public int duration;
    void Start()
    {
        Destroy(this.gameObject, duration);
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Player"))
            {
                print(collider.gameObject.name + " is getting pushed");
                // apply force on target towards me
                rb.AddExplosionForce(pushforce, transform.position, 0);
                print("adding force " + pushforce);
            }
        }
    }

    void Update()
    {

       
    }

    void OnDestroy()
    {
        CmdRemoveBomb();
    }

    [Command]
    void CmdRemoveBomb()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<MoveScript>().Respawn();
            }
        }
        NetworkServer.Destroy(this.gameObject);
    }



}