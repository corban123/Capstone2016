﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class ProjectileScript : NetworkBehaviour
{

    public float MoveSpeed = 5.0f;

    public float frequency = 20.0f;  // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement
    private Vector3 axis;
    private Vector3 pos;
    Rigidbody rb;
   
    void Start()
    {
        pos = transform.position;
        axis = transform.up;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (gameObject.name.Contains("Basic"))
        {
            //TODO: Add movement behaviors as the projectile travels
            rb.AddForce(transform.forward * MoveSpeed * 30);
        }
        else if (gameObject.name.Contains("Element"))
        {
            //TODO: Add movement behaviors as the projectile travels
            rb.AddForce(transform.forward * MoveSpeed * 30);
        }
        else //The projectile is a Quark, follows a sinusoidal path
        {
            pos += transform.forward * Time.deltaTime * MoveSpeed;
            transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
        }


    }

    void OnCollisionEnter(Collision coll)
    {
       //Destroy the projectile if it hits something
       if(coll.gameObject != this.gameObject)
        {
            Destroy(this.gameObject);
        }
    }


    void Update()
    {

    }
}
