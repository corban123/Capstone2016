using UnityEngine;
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
    public int heldElement; //This integer represents the current element held by the player, if the player is not holding an element set this value to -1
    private Text healthText;
    float startTime;
    bool takeDmg;
    private Text elementText;

    // Use this for initialization
    void Start () {
        haveElement = false;
        heldElement = -1;
        shotSpawn = gameObject.transform.GetChild(0);
        startTime = Time.time;
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        SetHealthText();
        takeDmg = false;
        elementText = GameObject.Find("ElementText").GetComponent<Text>();
        DeleteElementText();
    }
    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && isLocalPlayer) //PC control
        {
            nextFire = Time.time + fireRate;
            CmdShoot();
        }
        if (Time.time - startTime > 3)
        {
            takeDmg = true;
        }

    }

    void SetHealthText()
    {
        if (isLocalPlayer)
        {
            healthText.text = "Health:  " + numQuarks.ToString();
        }
    }

    void DeleteElementText()
    {
        if (isLocalPlayer)
        {
            elementText.text = "Element:  ---";
        }
    }

    public void SetElementText() 
    {
        if (isLocalPlayer)
        {
            elementText.text = "Element:  " + heldElement.ToString();
        }
    }

    //Will create a projectile based on what the player has available in the inventory
    [Command]
    public void CmdShoot()
    {
        GameObject instance;
        Vector3 newPos = shotSpawn.position;
        Quaternion newRot = this.gameObject.transform.GetChild(1).GetComponent<Camera>().transform.rotation;
        int playerNum = System.Int32.Parse(this.gameObject.name.Split(' ')[1]);
        if(numQuarks > 0)
        {
            numQuarks--;
            instance = Instantiate(shot, newPos, newRot) as GameObject;
        }
        else if(haveElement)
        {
            instance = Instantiate(elementShot, newPos, newRot) as GameObject;
            print(playerNum + " fires element " + heldElement);
            instance.GetComponent<ElementScript>().carrier = playerNum;
            instance.GetComponent<ElementScript>().elementID = heldElement;
            haveElement = false;
            heldElement = -1; //They shot the element, so it should be set back to null, this could be a potential issue depending on how we handle references to the elements because we might be removing the game object completely.
            DeleteElementText();
        }
        else
        {
            instance = Instantiate(basicShot, newPos, newRot) as GameObject;
        }
        instance.GetComponent<ProjectileScript>().playerSource = playerNum;
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
        if (collision.tag == "Bullet" && takeDmg)
        {
            ProjectileScript projectile = collision.GetComponent<ProjectileScript>();
            if (projectile.playerSource != this.gameObject.name.ToCharArray()[this.gameObject.name.Length-1])
            {
                string uIdentity = this.transform.name;
                CmdTellServerWhoWasShot(uIdentity);
            }
        }
    }

    void OnHealthChanged(int hlth)
    {
        numQuarks = hlth;
        SetHealthText();
    }
}
