using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AtomBomb : NetworkBehaviour {

    public float radius;
    private Collider[] victims;
    void Start()
    {      
        foreach (Collider collider in Physics.OverlapSphere(transform.position, radius))
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Player"))
            {
                print(rb.gameObject.name + " got caught in the explosion");
                rb.GetComponent<GUIScript> ().enableRespawning ();
                rb.GetComponent<MoveScript>().Respawn();
            }
        }
        
    }

    void Update()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(this.gameObject);
        }
    }


    [Command]
    void CmdRemoveBomb()
    {
        NetworkServer.Destroy(this.gameObject);
    }



}