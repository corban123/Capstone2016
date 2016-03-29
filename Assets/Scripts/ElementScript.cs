using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
/*
*   This script contains the logic for elements and their abilities/power ups, and will be used by elements that have been fired
*/

public class ElementScript : NetworkBehaviour
{
    public int cost;
    public int elementID;   //This id is unique to each element (numbered 1-16)
    public int carrier;     //This number is 1 or 2 depending on which player is holding it, or -1 depending on if nobody is holding it
    public GameObject blackHole;
    public GameObject atomBomb;
    public GameObject metallize;
    enum Element { Alkaline, Metals, Gases, Noble}
    Ray shootRay;
    RaycastHit shootHit;
    LineRenderer gunLine;
    public float range = 100f;

	// Use this for initialization
	void Start ()
    {

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
            CmdSpawnMetal();
        } else if (IsElementType () == Element.Noble) {
            RailGun ();
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

    [Command]
    void CmdSpawnMetal()
    {
        GameObject instance = Instantiate(metallize, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), transform.rotation) as GameObject;
        NetworkServer.Spawn(instance);
    }

    public void RailGun() {
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range))
        {
            gunLine.SetPosition(1, shootHit.point);
        }
        // If the raycast didn't hit anything...
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
