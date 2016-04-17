using UnityEngine;
using System;
using UnityEngine.Networking;
/*
*   This script contains the logic for elements and their abilities/power ups, and will be used by elements that have been fired
*/

public class ElementScript : NetworkBehaviour
{
    public int cost;
	[SyncVar]
	[SerializeField] public int elementID;   //This id is unique to each element (numbered 1-16)

    [SyncVar] public int carrier;     //This number is 1 or 2 depending on which player is holding it, or -1 depending on if nobody is holding it
    public GameObject blackHole;
    public GameObject atomBomb;
    enum Element { Alkaline, Metals, Gases, Noble}

	public GameObject barium;
	public GameObject calcium;
	public GameObject carbon;
	public GameObject copper;
	public GameObject gold;
	public GameObject helium;
	public GameObject hydrogen;
	public GameObject krypton;
	public GameObject neon;
	public GameObject nickel;
	public GameObject nitrogen;
	public GameObject oxygen;
	public GameObject potassium;
	public GameObject silver;
	public GameObject sodium;
	public GameObject xenon;
    Ray shootRay;
    RaycastHit shootHit;
    LineRenderer gunLine;
    public float range = 20f;
    GUIScript guiOtherPlayer;
	[SyncVar][SerializeField] public Vector3 spawnTrans;
	public GameObject elemPrefab;


	// Use this for initialization
	void Start ()
    {
        elementID = gameObject.GetComponent<ProjectileScript> ().elementId;
        carrier = gameObject.GetComponent<ProjectileScript> ().playerSource;
        print ("elementID " + elementID + " from player " + carrier);
	}
	
	// Update is called once per frame
	void Update ()
    { 
	}

    public void PowerUp()
    {
        if (elementID == null) {
            print ("ERROR: elementId undefined in ElementScript. Don't know which power up to spawn.");
        }
        else if (IsElementType () == Element.Alkaline) {
            CmdSpawnBlackHole ();
        } else if (IsElementType () == Element.Metals) {
            Freeze ();
        } else if (IsElementType () == Element.Noble) {
            Blackout ();
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
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range)) {
            FirstPersonController fpc = collider.GetComponent<FirstPersonController> ();
            if (fpc != null) {
                fpc.FreezeMovement ();
            }
        }
    }

    void Blackout() {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range)) {
            GUIScript gui = collider.GetComponent<GUIScript> ();
            if (gui != null) {
                gui.blackOutUI ();
            }
        }
    }
	[Command]
	public void CmdSpawnDead(){
		GameObject instance;
		elemPrefab.GetComponent<ElementScript>().elementID = elementID;
		elemPrefab.GetComponent<ElementScript> ().spawnTrans = spawnTrans;
		GameObject marker = GetObject (elementID);

		instance = Instantiate(elemPrefab, new Vector3(spawnTrans.x, spawnTrans.y + 4, spawnTrans.z), new Quaternion(0, 0, 0, 0)) as GameObject;
		marker = Instantiate (marker, new Vector3 (spawnTrans.x, spawnTrans.y + 10, spawnTrans.z), new Quaternion (0, 0, 0, 0)) as GameObject;
		marker.transform.parent = instance.transform;
		NetworkServer.Spawn(instance);
		NetworkServer.Spawn (marker);
		Debug.Log ("In CmdSPawnDead");

	}
	public GameObject GetObject(int element) {
		switch (element) {
		case 0:
			return sodium;
		case 1:
			return potassium;
		case 2:
			return calcium;
		case 3:
			return barium;
		case 4:
			return copper;
		case 5:
			return nickel;
		case 6:
			return silver;
		case 7:
			return gold;
		case 8:
			return carbon;
		case 9:
			return nitrogen;
		case 10:
			return oxygen;
		case 11:
			return hydrogen;
		case 12:
			return helium;
		case 13:
			return neon;
		case 14:
			return krypton;
		case 15:
			return xenon;
		default:
			return null;
		}
	}
}
