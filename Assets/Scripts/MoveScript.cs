using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
/*
* A script for moving the character and rotating the FPC camera.
*/
public class MoveScript : NetworkBehaviour
{
    CombatScript combat;
    AudioSource source;
    GUIScript gui;

    [SerializeField] AudioClip pickUpQuark;
    [SerializeField] AudioClip pickUpElement;
    [SerializeField] AudioClip cantPickUp;


    float upDownRange = 60.0f;
    Transform player1RespawnPoint;
    Transform player2RespawnPoint;
	public GameObject marker;

    public Sprite blueCross;
    public Sprite redCross;

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



    // Use this for initialization
    void Start()
    {
        combat = gameObject.GetComponent<CombatScript>();
        source = gameObject.GetComponent<AudioSource>();
        gui = gameObject.GetComponent<GUIScript>();
        player1RespawnPoint = GameObject.Find ("Spawn1").transform;
        player2RespawnPoint = GameObject.Find ("Spawn2").transform;
        //Respawn();
    }

	void callThis(){
	
		Debug.Log ("CALLED THIS");
	
	}

    void Update()
    {
        
        if(isLocalPlayer)
        {
            combat = gameObject.GetComponent<CombatScript>();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Player"))
            {
                GameObject.Find("Crosshair").GetComponent<Image>().sprite = redCross;
            }
            else
            {
                GameObject.Find("Crosshair").GetComponent<Image>().sprite = blueCross;
            }
        }
        
    }
    void FixedUpdate()
    {
        
    }
    // Pick up elements and quarks on collision
    void OnTriggerEnter(Collider collision)
    {
        if (isLocalPlayer)
        {
            if (collision.tag == "Element" && combat.heldElement == -1 && collision.GetComponent<ElementScript> ().cost <= combat.numQuarks) {
                source.PlayOneShot (pickUpElement, 1.0f);

                GetComponent<CombatScript> ().CmdDeductElementCostQuarks ();
                GameObject pickedElement = collision.gameObject;
                combat.haveElement = true;
                combat.heldElement = pickedElement.GetComponent<ElementScript> ().elementID;
                combat.heldElementPos = pickedElement.GetComponent<ElementScript> ().spawnTrans;
                CmdPickUpElement (pickedElement);
                CmdSpawnMarker (new Vector3 (this.transform.position.x, this.transform.position.y + 5, transform.position.z), combat.heldElement, this.gameObject.name);

                print ("picked up element " + combat.heldElement);
                gui.SetElementUI (combat.heldElement);
                gui.enableElementPickedUp ();
                Destroy (pickedElement);
            } else if (collision.tag == "Element" && (combat.heldElement != -1 || collision.GetComponent<ElementScript> ().cost > combat.numQuarks)) {
                source.PlayOneShot(cantPickUp, 1.0f);
            }

            if (collision.tag == "Quark")
            {
                source.PlayOneShot(pickUpQuark, 1.0f);

                GameObject pickedQuark = collision.gameObject;
                print("picked up quark");
                combat.CmdAddQuarks();
                CmdPickUpQuark (pickedQuark);
                Destroy(pickedQuark);
            }


            if (collision.tag == "Killbox")
            {
                print("you died :(");
                Respawn();
            }
        }
    }

    [Command]
    void CmdPickUpElement(GameObject element)
    {
        NetworkServer.Destroy(element);
    }

    [Command]
    void CmdPickUpQuark(GameObject quark)
    {
        NetworkServer.Destroy(quark);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        //Debug.Log ("W: " + q.w);

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -upDownRange, upDownRange);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public void Respawn()
    {
        gameObject.GetComponent<FirstPersonController>().moveFactor = 1.0f;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
		if (isLocalPlayer) {
            combat.CmdDeletAllQuarks();
            combat.CmdDeleteElement();
        }
        if (isLocalPlayer)
        {
            if (gameObject.name == "Player 1")
            {

                print("Player 1 died");
                transform.position = player1RespawnPoint.transform.position;
            }
            else {
                print("Player 2 died");
                transform.position = player2RespawnPoint.transform.position;
            }
        }
    }

    private void addYourElement(GameObject marked)
    {
        marker = marked;

    }

    [Command]
	public void CmdSpawnMarker(Vector3 trans, int val, string name){
		GameObject marker = GetObject (val);
		marker = Instantiate (marker, new Vector3 (trans.x, trans.y + 10, trans.z), new Quaternion (0, 0, 0, 0)) as GameObject;
		marker.transform.parent = this.transform;
		marker.GetComponent<ElementMarkerScript> ().enabled = false;
		marker.transform.rotation = new Quaternion (0, 0, 0, 0);
        addYourElement(marker);
		//marker.GetComponent<ElementMarkerPlayer> ().enabled = true;
		NetworkServer.Spawn (marker);
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