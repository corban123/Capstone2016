﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
/*
*   This script contains the logic for elements and their abilities/power ups, and will be used by elements that have been fired
*/

public class ElementScript : NetworkBehaviour
{
    public int cost;
	[SyncVar]
	[SerializeField] public int elementID = 0;   //This id is unique to each element (numbered 1-16)

    public int carrier;     //This number is 1 or 2 depending on which player is holding it, or -1 depending on if nobody is holding it
    public GameObject blackHole;
    public GameObject atomBomb;
    enum Element { Alkaline, Metals, Gases, Noble}
    Ray shootRay;
    RaycastHit shootHit;
    LineRenderer gunLine;
    public float range = 100f;
    GUIScript gui;


	// Use this for initialization
	void Start ()
    {
        gui = gameObject.GetComponent<GUIScript> ();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void PowerUp()
    {
        if (IsElementType () == Element.Alkaline) {
            CmdSpawnBlackHole ();
        } else if (IsElementType () == Element.Metals) {
            Freeze ();
        } else if (IsElementType () == Element.Noble) {
            gui.blackOutUI ();
        }
        else {
            CmdSpawnBomb();
        }
    }

    Element IsElementType() //Useful to know what kind of element this script is attached to
    {
        if(elementID >= 0 && elementID <= 3)
        {
            return Element.Alkaline;
        }
        else if (elementID >= 4 && elementID <= 7)
        {
            return Element.Metals;
        }
        else if (elementID >= 8 && elementID <= 11)
        {
            return Element.Gases;
        }
        else
        {
            return Element.Noble;
        }
    }

    [Command]
    void CmdSpawnBlackHole()
    {
        GameObject instance = Instantiate(blackHole, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), transform.rotation) as GameObject;
        NetworkServer.Spawn(instance);
    }

    [Command]
    void CmdSpawnBomb()
    {
        GameObject instance = Instantiate(atomBomb, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), transform.rotation) as GameObject;
        NetworkServer.Spawn(instance);
    }

    void Freeze() {
        float range = 50f;

        foreach (Collider collider in Physics.OverlapSphere(transform.position, range)) {
            FirstPersonController fpc = collider.GetComponent<FirstPersonController> ();
            if (fpc != null) {
                fpc.FreezeMovement ();
            }
        }
    }
}
