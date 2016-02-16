﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class CombatScript : NetworkBehaviour
{
    public GameObject shot;
    public GameObject elementShot;
    public GameObject basicShot;
    Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    [SyncVar (hook = "OnHealthChanged")] public int numQuarks = 0;
    public bool haveElement;
    public GameObject heldElement; //This GameObject represents the current element held by the player, if the player is not holding an element set this value to null
    private Text healthText;

    // Use this for initialization
    void Start () {
        haveElement = false;
        heldElement = null;
        shotSpawn = gameObject.transform.GetChild(0);
        Debug.Log("Object" + GameObject.Find("HealthText").name);

        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        SetHealthText();
    }
    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && isLocalPlayer) //PC control
        {
            nextFire = Time.time + fireRate;
            CmdShoot();
        }
    }

    void SetHealthText()
    {
        if (isLocalPlayer)
        {
            healthText.text = "Health" + numQuarks.ToString();
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
            instance = Instantiate(shot, shotSpawn.position, this.gameObject.transform.GetChild(2).GetComponent<Camera>().transform.rotation) as GameObject;
        }
        else if(haveElement)
        {
            instance = Instantiate(elementShot, shotSpawn.position, this.gameObject.transform.GetChild(2).GetComponent<Camera>().transform.rotation) as GameObject;
            haveElement = false;
            heldElement = null; //They shot the element, so it should be set back to null, this could be a potential issue depending on how we handle references to the elements because we might be removing the game object completely.
        }
        else
        {
            instance = Instantiate(basicShot, shotSpawn.position, this.gameObject.transform.GetChild(2).GetComponent<Camera>().transform.rotation) as GameObject;
        }
        NetworkServer.Spawn(instance);
    }

    [Command]
    void CmdTellServerWhoWasShot(string uniqueID)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<CombatScript>().DeductHealth();

    }

    void DeductHealth()
    {
        numQuarks = numQuarks / 2;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet")
        {
            string uIdentity = this.transform.name;
            CmdTellServerWhoWasShot(uIdentity);
        }
    }

    void OnHealthChanged(int hlth)
    {
        numQuarks = hlth;
        SetHealthText();
    }
}
