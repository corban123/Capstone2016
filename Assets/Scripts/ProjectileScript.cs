using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

/*
*   This script controls the behavior of all projectiles as they travel, and removes the projectile if they collide with something.
*/
public class ProjectileScript : NetworkBehaviour
{
    [SyncVar]public int playerSource;
    [SyncVar]public int elementId;
    public float MoveSpeed = 5.0f;
    public float frequency = 20.0f;  // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement
    private Vector3 axis;
    private Vector3 pos;
    AudioSource source;
    [SerializeField] AudioClip shoot;
    Rigidbody rb;
    private int delay = 5;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = shoot;
        source.Play();
        pos = transform.position;
        axis = transform.up;
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, delay);
    }

    void FixedUpdate()
    {
        if (gameObject.name.Contains("Basic"))
        {
            //TODO: Add movement behaviors as the projectile travels
            rb.AddForce(transform.forward * MoveSpeed * 30);
        }
        else if (gameObject.name.Contains("Element"))
        {
            //TODO: Add movement behaviors as the projectile travels
            rb.AddForce(transform.forward * MoveSpeed * 30);
        }
        else //The projectile is a Quark, follows a sinusoidal path
        {
            pos += transform.forward * Time.deltaTime * MoveSpeed;
            transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        ElementScript e = GetComponent<ElementScript>();

        //Destroy the projectile if it hits something
        Transform targetParent = coll.gameObject.transform.parent;
        GameObject target = coll.gameObject;
        try
        {
            //Checks if projectile is colliding with player that shot it
            if (!(target.CompareTag("Player")))
            {
                //If the projectile has a element script, then it's an element. It needs to active it's powerup on collision.
                if (e != null && !coll.gameObject.CompareTag("Base"))
                {
                    e.PowerUp();                       
                } else if (e != null && coll.gameObject.CompareTag("Base")){
                    int BaseId = Int32.Parse(coll.gameObject.name.Split(' ')[1]);
                    print("base " + BaseId + " hit by player " + playerSource + " with element " + elementId);
                    if (playerSource == BaseId)
                    {
                        print("Player " + playerSource);
                        BoardScript board = GameObject.Find("Player " + playerSource).GetComponent<BoardScript>();
                        GUIScript gui = GameObject.Find("Player " + playerSource).GetComponent<GUIScript>();
                        print("bs " + board);
                        bool isWin = board.score(elementId);

                        if(isWin)
                            gui.enableYouWon ();
                        else
                            gui.enableYouScored ();

//                        if (playerSource == 1) {
//                            GUIScript otherGui = GameObject.Find("Player 2").GetComponent<GUIScript>();
//                            otherGui.enableEnemyScored ();
//
//                        } else if (playerSource == 2) {
//                            GUIScript otherGui = GameObject.Find("Player 1").GetComponent<GUIScript>();
//                            otherGui.enableEnemyScored ();
//                        }
                    }
                }
                Destroy(this.gameObject);
            }            
        }
        catch (NullReferenceException) { }
    }


    void Update()
    {

    }

    void OnDestroy()
    {
        //CmdRemoveProjectile();
        if (!gameObject.name.Contains("Element") && !gameObject.name.Contains("Basic"))
        {
            GameObject.Find("GenerateBoard").GetComponent<QuarkOverlord>().deSpawn();
        }
    }

    [Command]
    void CmdRemoveProjectile()
    {
        NetworkServer.UnSpawn(this.gameObject);
    }
}
