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
    readonly int elementCost = 5;

    [SyncVar (hook = "OnHealthChanged")] public int numQuarks = 0;
    public bool haveElement;
    public int heldElement; //This integer represents the current element held by the player, if the player is not holding an element set this value to -1
	public Vector3 heldElementPos;
    float startTime;
    public bool takeDmg;

    readonly int elementPickUpCost = 5;

    GUIScript gui;
    PauseScript pauseScript;

    // Use this for initialization
    void Start () {
        haveElement = false;
        heldElement = -1;
		heldElementPos = Vector3.zero;
        startTime = Time.time;
        takeDmg = false;

        gui = gameObject.GetComponent<GUIScript> ();
        pauseScript = gameObject.GetComponent<PauseScript> ();
    }
    
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) {
            if (Input.GetButtonDown ("Fire1") && Time.time > nextFire && !pauseScript.paused) { //PC control
                // This boolean has to be before CmdShootProjectile because numQuarks is decremented in the command.
                bool shootElement = haveElement && numQuarks <= 0;
                nextFire = Time.time + fireRate;
                CmdShootProjectile (shootElement, heldElement, heldElementPos);
                // These lines have to be outside CmdShootProjectile. If they are inside the command they will NOT work.
                if (shootElement) {
                    haveElement = false;
                    heldElement = -1;
                    gui.DeleteElementUI ();
                }
            }
            
            else if(Input.GetButtonDown("Fire2") && Time.time > nextFire && isLocalPlayer && !pauseScript.paused && haveElement)
            {
                nextFire = Time.time + fireRate;
                CmdDeletAllQuarks ();
				CmdDestroyMarker ();
				Destroy (this.GetComponent<MoveScript> ().marker);
				this.GetComponent<MoveScript> ().marker = null;
                CmdShootProjectile(haveElement, heldElement, heldElementPos);
                haveElement = false;
                heldElement = -1;
                gui.DeleteElementUI();
            }
            
            if (Time.time - startTime > 3) {
                takeDmg = true;
            }
        }
    }
	[Command]
	void CmdDestroyMarker(){
		NetworkServer.Destroy (this.GetComponent<MoveScript> ().marker);

	
	
	}

    [Command]
    public void CmdDeletAllQuarks() {
        for (int i = 0; i < numQuarks; i++)
        {
            GameObject.Find("GenerateBoard").GetComponent<QuarkOverlord>().deSpawn();
        }
        numQuarks = 0;
    }

    [Command]
    public void CmdDeductElementCostQuarks() {
        for (int i = 0; i < elementPickUpCost; i++)
        {
            GameObject.Find("GenerateBoard").GetComponent<QuarkOverlord>().deSpawn();
        }
        numQuarks -= elementPickUpCost;
    }

    [Command]
    public void CmdDeleteQuarks() {
        
        GameObject.Find("GenerateBoard").GetComponent<QuarkOverlord>().deSpawn();
        
        numQuarks--;
    }

    [Command]
    public void CmdAddQuarks(){
        numQuarks++;
    }
    [Command]
    public void CmdHitQuark()
    {
        for (int i = 0; i < numQuarks/2; i++)
        {
            GameObject.Find("GenerateBoard").GetComponent<QuarkOverlord>().deSpawn();
        }
        numQuarks = numQuarks / 2;

    }


    [Command]
	void CmdShootProjectile(bool shootElementCmd, int heldElementCmd, Vector3 heldElementPos)
    {
		GameObject instance;
		Vector3 newPos = shotSpawn.position;
		Quaternion newRot = transform.Find("FirstPersonCharacter").GetComponent<Camera>().transform.rotation;
		int playerNum = System.Int32.Parse(this.gameObject.name.Split(' ')[1]);
		gameObject.GetComponent<Animator>().Play("Shoot");
		
		if (shootElementCmd) {
			instance = Instantiate(elementShot, newPos, newRot) as GameObject;
            instance.GetComponent<ProjectileScript> ().elementId = heldElementCmd;
			instance.GetComponent<ElementScript> ().spawnTrans = heldElementPos;
		}
		else
		{
			if (numQuarks > 0)
			{
				instance = Instantiate(quarkShot, newPos, newRot) as GameObject;
				print("shooting");
                CmdDeleteQuarks();
			}
			else
			{
				instance = Instantiate(basicShot, newPos, newRot) as GameObject;
			}
			
		}
        instance.GetComponent<ProjectileScript>().playerSource = playerNum;
		NetworkServer.Spawn(instance);
    }
    
    [Command]
    void CmdTellServerWhoWasShot(string uniqueID, string bullet)
    {
        GameObject go = GameObject.Find(uniqueID);
        go.GetComponent<CombatScript>().DeductHealth(bullet);
    }
    
    /**
     * Deduct the health by dividing the number of quarks by 2.
     */
    void DeductHealth(string bullet)
    {
        if (isLocalPlayer)
        {
            if (numQuarks < 1)
            {
				gameObject.GetComponent<Animator>().Play("Death");
			}
            else if(bullet.Contains("Basic"))
            {
                CmdDeleteQuarks();
            }
            else
            {
                CmdHitQuark();
            }
            gui.updateQuarkMeter(numQuarks);
        }
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

                DeductHealth(collision.name);
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
            if (numQuarks >= elementPickUpCost) {
                gui.enableGaugeGlow ();
            } else {
                gui.disableGaugeGlow ();
            }
        }
    }
}
