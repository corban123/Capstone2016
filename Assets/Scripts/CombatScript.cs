﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class CombatScript : NetworkBehaviour
{
    public GameObject shot;
    public GameObject elementShot;
    public GameObject basicShot;
    Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    public int numQuarks;
    public bool haveElement;
    public GameObject heldElement; //This GameObject represents the current element held by the player, if the player is not holding an element set this value to null

    // Use this for initialization
    void Start () {
        haveElement = false;
        heldElement = null;
        numQuarks = 0;
        shotSpawn = gameObject.transform.GetChild(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && isLocalPlayer) //PC control
        {
            nextFire = Time.time + fireRate;
            CmdShoot();
        }
    }

    //Will create a projectile based on what the player has available in the inventory
    [Command]
    public void CmdShoot()
    {
        GameObject instance;
        if(numQuarks > 0)
        {
            numQuarks--;
            instance = Instantiate(shot, shotSpawn.position, Camera.main.transform.rotation) as GameObject;
        }
        else if(haveElement)
        {
            instance = Instantiate(elementShot, shotSpawn.position, Camera.main.transform.rotation) as GameObject;
            haveElement = false;
            heldElement = null; //They shot the element, so it should be set back to null, this could be a potential issue depending on how we handle references to the elements because we might be removing the game object completely.
        }
        else
        {
            instance = Instantiate(basicShot, shotSpawn.position, Camera.main.transform.rotation) as GameObject;
        }
        NetworkServer.Spawn(instance);
    }
}
