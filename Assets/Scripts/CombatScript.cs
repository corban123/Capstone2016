using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class CombatScript : NetworkBehaviour
{
    public GameObject quarkShot;
    public GameObject elementShot;
    public GameObject basicShot;
    public Transform shotSpawn;
    private float nextFire;
    public float fireRate;

    [SyncVar (hook = "OnHealthChanged")] public int numQuarks = 0;
    public bool haveElement;
    public int heldElement; //This integer represents the current element held by the player, if the player is not holding an element set this value to -1

    float startTime;
    public bool takeDmg;

    readonly int elementPickUpPrice = 5;

    GUIScript gui;

    // Use this for initialization
    void Start () {
        haveElement = false;
        heldElement = -1;

        startTime = Time.time;
        takeDmg = false;

        gui = gameObject.GetComponent<GUIScript> ();
    }
    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && isLocalPlayer) //PC control
        {
            gameObject.GetComponent<Animator>().Play("Shoot");
            nextFire = Time.time + fireRate;
            Shoot ();
        }
        else if(Input.GetButtonDown("Fire2") && Time.time > nextFire && isLocalPlayer)
        {
            gameObject.GetComponent<Animator>().Play("Shoot");
            nextFire = Time.time + fireRate;
            ShootElement();
        }
        if (Time.time - startTime > 3)
        {
            takeDmg = true;
        }
    }

    [Command]
    void CmdDeleteQuarks() {
        numQuarks--;
    }

    void Shoot() {
        GameObject instance;
        Vector3 newPos = shotSpawn.position;
        Quaternion newRot = transform.Find("FirstPersonCharacter").GetComponent<Camera>().transform.rotation;
        int playerNum = System.Int32.Parse(this.gameObject.name.Split(' ')[1]);
        if(numQuarks > 0)
        {
            instance = Instantiate(quarkShot, newPos, newRot) as GameObject;
            print ("shooting");
            CmdDeleteQuarks ();
        }
        else if(haveElement)
        {
            instance = Instantiate(elementShot, newPos, newRot) as GameObject;
            print(playerNum + " fires element " + heldElement);

            instance.GetComponent<ElementScript>().carrier = playerNum;
            instance.GetComponent<ElementScript>().elementID = heldElement;
            haveElement = false;
            heldElement = -1;
            gui.DeleteElementUI ();
        }
        else
        {

            instance = Instantiate(basicShot, newPos, newRot) as GameObject;
        }
        instance.GetComponent<ProjectileScript>().playerSource = playerNum;
        CmdShoot (instance);
    }

    void ShootElement()
    {
        if (haveElement)
        {
            Vector3 newPos = shotSpawn.position;
            Quaternion newRot = transform.Find("FirstPersonCharacter").GetComponent<Camera>().transform.rotation;
            GameObject instance = Instantiate(elementShot, newPos, newRot) as GameObject;
            int playerNum = System.Int32.Parse(this.gameObject.name.Split(' ')[1]);

            instance.GetComponent<ElementScript>().carrier = playerNum;
            instance.GetComponent<ElementScript>().elementID = heldElement;
            numQuarks = 0;
            haveElement = false;
            heldElement = -1;
            gui.DeleteElementUI();
        }
    }
       

    //Will create a projectile based on what the player has available in the inventory
    [Command]
    public void CmdShoot(GameObject instance)
    {
        NetworkServer.Spawn(instance);
    }

    [Command]
    void CmdTellServerWhoWasShot(string uniqueID)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<CombatScript>().DeductHealth();

    }

    /**
     * Deduct the health by dividing the number of quarks by 2.
     */
    void DeductHealth()
    {
		if (numQuarks < 1) {

			//this.gameObject.GetComponent<MoveScript> ().Respawn ();
		} else {
			numQuarks = numQuarks / 2;
		}
        gui.updateQuarkMeter (numQuarks);
    }

    /**
     * If the player is shot by a bullet and they can be hit, tell the server that they were shot.
     */
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet" && takeDmg)
        {
			Debug.Log ("WELL I'M IN THE COLLISION");
            ProjectileScript projectile = collision.GetComponent<ProjectileScript>();
            if (projectile.playerSource != this.gameObject.name.ToCharArray()[this.gameObject.name.Length-1])
            {
				Debug.Log ("WELL I GOT HIT DIDN'T I");
                string uIdentity = this.transform.name;
                CmdTellServerWhoWasShot(uIdentity);
            }
        }
    }

    public void OnHealthChanged(int hlth)
    {
        print ("on change");
        if (isLocalPlayer) {
            print ("update health");
            numQuarks = hlth;
            gui.updateQuarkMeter (numQuarks);
            if (numQuarks >= elementPickUpPrice) {
                gui.enableGaugeGlow ();
            } else {
                gui.disableGaugeGlow ();
            }
        }
    }
}
