﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AtomBomb : NetworkBehaviour {

    public float range;
    private Collider[] victims;
    void Start()
    {      
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null && rb.gameObject.CompareTag("Player"))
            {
                print(rb.gameObject.name + " got caught in the explosion");
                rb.GetComponent<MoveScript>().Respawn();
                rb.GetComponent<MoveScript>().showRespawn();
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