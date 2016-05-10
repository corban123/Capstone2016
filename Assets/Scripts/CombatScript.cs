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

    readonly int elementPickUpCost = 5;

    GUIScript gui;
    PauseScript pauseScript;

    NetworkIdentity myId;
    SkinnedMeshRenderer[] smr;

    [SerializeField] AudioClip mPain;
    [SerializeField] AudioClip fPain;

    AudioSource source;
    // Use this for initialization
    void Start () {
        haveElement = false;
        heldElement = -1;
		heldElementPos = Vector3.zero;
        myId = gameObject.GetComponent<NetworkIdentity>(); // get the object's network ID
        smr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        source = gameObject.GetComponent<AudioSource>();
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
        
        
        numQuarks--;
    }
    [Command]
    public void CmdDeleteHitQuarks()
    {
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
    public void CmdDeleteElement()
    {
        haveElement = false;
        heldElement = -1;
        gui.DeleteElementUI();
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
                gameObject.GetComponent<GUIScript> ().enableRespawning ();
				gameObject.GetComponent<Animator>().Play("Death");
			}
            else if(haveElement == true)
            {
                CmdDeleteElement();
            }
            else if(bullet.Contains("Basic"))
            {
                CmdDeleteHitQuarks();
            }
            
            else
            {
                CmdHitQuark();
            }
            gui.updateQuarkMeter(numQuarks);
        }
    }

    IEnumerator DamageFlash()
    {
        CmdPaint(Color.red);
        yield return new WaitForSeconds(.5f);
        CmdPaint(Color.white);
    }


    [ClientRpc]
    void RpcPaint(Color col)
    {
        foreach (SkinnedMeshRenderer render in smr)
        {
            foreach (Material mat in render.materials)
            {
                if (mat.name.Contains("Labcoat"))
                {
                    mat.SetColor("_Color", col);
                }
            }
        }
    }

    [Command]
    void CmdPaint(Color col)
    {
        RpcPaint(col);       // usse a Client RPC function to "paint" the object on all clients
    }

    /**
     * If the player is shot by a bullet and they can be hit, tell the server that they were shot.
     */
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet")
        {
            ProjectileScript projectile = collision.GetComponent<ProjectileScript>();
            if (!gameObject.name.Contains(projectile.playerSource.ToString()))
            {
                string uIdentity = this.transform.name;
                if (gameObject.name.Contains("1")) { source.PlayOneShot(mPain, 1.0f); }
                else { source.PlayOneShot(fPain, 1.0f); }
                StartCoroutine("DamageFlash");
                DeductHealth(collision.name);
            }
        }
        
    }

    public void OnHealthChanged(int hlth)
    {
        if (isLocalPlayer) {
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
