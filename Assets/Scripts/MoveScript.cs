﻿using UnityEngine;
using System.Collections;

/*
* A script for moving the character and rotating the FPC camera.
*/
public class MoveScript : MonoBehaviour
{
    CombatScript combat;
	AudioSource source;
	[SerializeField] AudioClip pickUp;
	// Use this for initialization
	void Start ()
    {
        combat = gameObject.GetComponent<CombatScript>();
		source = gameObject.GetComponent<AudioSource> ();
	}

	void Update () 
	{
    }

	// Rotate the first person camera by veritcalRotation degrees in the vertical direction.
	// Rotate the first person character by leftRight degrees in the horizontal direction.
    public void RotateCharacter(float verticalRotation, float leftRight)
    {
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(0, leftRight, 0);
    }

	// Move the given CharacterController by the vector speed.
    public void MoveCharacter(CharacterController characterController, Vector3 speed)
    {
        characterController.Move(speed);
    }

	// Pick up elements and quarks on collision
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Element" && combat.heldElement == null)
        {
            combat.haveElement = true;
            print("picked up element");
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Quark")
        {
            print("picked up quark");
            Destroy(collision.gameObject);
            combat.numQuarks += 1;
        }
		source.clip = pickUp;
		source.Play ();
    }
}
