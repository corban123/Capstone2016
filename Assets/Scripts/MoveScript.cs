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

    [SerializeField]
    AudioClip pickUp;
    float upDownRange = 60.0f;
    Quaternion nextCameraRotation;
    Quaternion nextPlayerRotation;
    public Transform player1RespawnPoint;
    public Transform player2RespawnPoint;

    // Use this for initialization
    void Start()
    {
        combat = gameObject.GetComponent<CombatScript>();
        source = gameObject.GetComponent<AudioSource>();
        gui = gameObject.GetComponent<GUIScript>();
        nextCameraRotation = this.gameObject.GetComponentInChildren<Camera>().transform.localRotation;
        nextPlayerRotation = transform.localRotation;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        //this.gameObject.transform.GetChild(0).transform.rotation = this.gameObject.transform.GetChild(2).GetComponent<Camera>().transform.rotation;
    }
    // Pick up elements and quarks on collision
    void OnTriggerEnter(Collider collision)
    {
        if (isLocalPlayer)
        {
            if (collision.tag == "Element" && combat.heldElement == -1)
            {
                GetComponent<CombatScript>().numQuarks -= collision.GetComponent<ElementScript>().cost;
                GameObject pickedElement = collision.gameObject;
                this.gameObject.GetComponent<CombatScript>().haveElement = true;
                this.gameObject.GetComponent<CombatScript>().heldElement = pickedElement.GetComponent<ElementScript>().elementID;
                CmdPickUpElement(pickedElement);
                print("picked up element " + combat.heldElement);
                gui.SetElementUI(combat.heldElement);
                gui.enableElementPickedUp();
                Destroy(pickedElement);
            }
            if (collision.tag == "Quark")
            {
                GameObject pickedQuark = collision.gameObject;
                print("picked up quark");
                this.gameObject.GetComponent<CombatScript>().AddQuarks();
                CmdPickUpQuark (pickedQuark);
                Destroy(pickedQuark);
            }
            source.clip = pickUp;
            source.Play();

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
			combat.numQuarks = 3;
		}

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