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

	[SerializeField] AudioClip pickUp;
    [SerializeField] AudioClip walk;
	[SerializeField] AudioClip jumpUp;
	float upDownRange = 60.0f;
	Quaternion nextCameraRotation;
	Quaternion nextPlayerRotation;
    public Transform player1RespawnPoint;
    public Transform player2RespawnPoint;
    
	// Use this for initialization
	void Start ()
    {
        combat = gameObject.GetComponent<CombatScript>();
		source = gameObject.GetComponent<AudioSource> ();
        gui = gameObject.GetComponent<GUIScript> ();
		nextCameraRotation = Camera.main.transform.localRotation;
		nextPlayerRotation = transform.localRotation;
	}

	void Update () 
	{
    }

    void FixedUpdate()
    {
        //this.gameObject.transform.GetChild(0).transform.rotation = this.gameObject.transform.GetChild(2).GetComponent<Camera>().transform.rotation;
    }

	// Rotate the first person camera by veritcalRotation degrees in the vertical direction.
	// Rotate the first person character by leftRight degrees in the horizontal direction.
    public void RotateCharacter(float verticalRotation, float leftRight)
    {	
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		nextCameraRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
		nextPlayerRotation *= Quaternion.Euler (0f, leftRight*5, 0f);
		nextCameraRotation = ClampRotationAroundXAxis (nextCameraRotation);
		Camera.main.transform.localRotation =  Quaternion.Lerp(Camera.main.transform.localRotation, nextCameraRotation , Time.fixedDeltaTime * 5f);
		transform.localRotation = Quaternion.Lerp (transform.localRotation, nextPlayerRotation, Time.fixedDeltaTime * 5f);
	}

	// Move the given CharacterController by the vector speed.
    public void MoveCharacter(CharacterController characterController, Vector3 speed)
    {
		if ((speed.x != 0 && characterController.isGrounded) || (speed.z != 0 && characterController.isGrounded)) {
			if (source.clip != walk) {
				source.loop = true;
				source.clip = walk;
				source.Play ();
			}
		} else if (((speed.x == 0 && speed.z == 0) && speed.y <= 0)) {
			source.loop = false;
			source.Stop ();
		} if(speed.x == 0 && speed.y <= 0 && speed.z ==0){
			source.clip = null;
		}
		if (!characterController.isGrounded && source.clip == walk) {
			source.clip = null;
		
		}
        characterController.Move(speed);
    }

	// Pick up elements and quarks on collision
    void OnTriggerEnter(Collider collision)
    {
		if (isLocalPlayer) {
			if (collision.tag == "Element" && combat.heldElement == -1 && GetComponent<CombatScript> ().numQuarks >= collision.GetComponent<ElementScript> ().cost) {
				GetComponent<CombatScript> ().numQuarks -= collision.GetComponent<ElementScript> ().cost;
				GameObject pickedElement = collision.gameObject;
				CmdPickUpElement (this.gameObject.name, pickedElement);
				print ("picked up element " + combat.heldElement);
				gui.SetElementUI (combat.heldElement);
				gui.enableElementPickedUp ();
				Destroy (pickedElement);
			}
			if (collision.tag == "Quark") {
				GameObject pickedQuark = collision.gameObject;
				print ("picked up quark");
				CmdPickUpQuark (this.gameObject.name, pickedQuark);
				Destroy (pickedQuark);
			}
			source.clip = pickUp;
			source.Play ();

			if (collision.tag == "Killbox") {
				print ("you died :(");
				Respawn ();
			}
		}
    }
    
    [Command]
    void CmdPickUpElement(string uniqueID, GameObject element)
    {
        this.gameObject.GetComponent<CombatScript>().haveElement = true;
        this.gameObject.GetComponent<CombatScript>().heldElement = element.GetComponent<ElementScript>().elementID;
        NetworkServer.Destroy(element);
    }

    [Command]
    void CmdPickUpQuark(string uniqueID, GameObject quark)
    {
        GameObject target = GameObject.Find(uniqueID);
        target.GetComponent<CombatScript>().numQuarks++;
        NetworkServer.Destroy(quark);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		//Debug.Log ("W: " + q.w);

		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, -upDownRange, upDownRange);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

    public void Respawn() {
        gameObject.GetComponent<FirstPersonController>().moveFactor = 1.0f;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        if (gameObject.name == "Player 1") {
            print ("Player 1 died");
            transform.position = player1RespawnPoint.transform.position;
        } else {
            print ("Player 2 died");
            transform.position = player2RespawnPoint.transform.position;
        }
    }
}
