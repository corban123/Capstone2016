﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

/*
*   This script controls the behavior of all projectiles as they travel, and removes the projectile if they collide with something.
*/
public class ProjectileScript : NetworkBehaviour
{
    public int playerSource;
    public float MoveSpeed = 5.0f;
    public float frequency = 20.0f;  // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement
    private Vector3 axis;
    private Vector3 pos;
    AudioSource source;
    [SerializeField] AudioClip shoot;
    Rigidbody rb;
    private int delay = 3;
   
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = shoot;
        source.Play();
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
        Destroy (gameObject, delay);
    }

    void OnTriggerEnter(Collider coll)
    {
        ElementScript e = GetComponent<ElementScript>();
        print("colliding with " + coll.gameObject.name);
        //Destroy the projectile if it hits something
        Transform targetParent = coll.gameObject.transform.parent;
        GameObject target = coll.gameObject;
        try
        {
            if (!(target.name.Contains("Player") && target.name.Contains(playerSource.ToString()))) //Checks if projectile is colliding with player that shot it
            {
                if (!(targetParent.name.Contains("Player") && targetParent.name.Contains(playerSource.ToString())))
                {
                    if (e != null && !coll.gameObject.name.Contains("Base")) //If the projectile has a element script, then it's an element. It needs to active it's powerup on collision.
                    {
                        e.PowerUp();
                        print("powering up...");
                    }
                    Destroy(this.gameObject);
                }
            }
        }
        catch (NullReferenceException) { }
    }


    void Update()
    {

    }
}
