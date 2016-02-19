﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/*
 * A class for handling player inputs.
 * 
 * Also handles death from falling off level.
 */
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : NetworkBehaviour 
{
    public float movementSpeed = 23.0f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 2.0f;
    float forwardSpeed;
    float sideSpeed;
    float leftRight;

	private float reallySmallNumber = -0.25f;

    private float nextFire;
    public float fireRate;

    [SerializeField] Camera FPSCam;
    [SerializeField] AudioListener audioListen;

    float verticalRotation = 0;
    public float upDownRange = 60.0f;

    float gravity = Physics.gravity.y / 1.5f;
	bool jump = false;
    public float verticalVelocity;

    Vector3 speed = Vector3.zero;

    CharacterController characterController;
    MoveScript move;
    CombatScript combat;

    // Initalize variables for this character
    void Start()
    {
        if (isLocalPlayer)
        {
			SetCursorState ();

			// Initalize variables
            leftRight = 0;
			verticalVelocity = 0;

			// Enable the character controller for this player
            characterController = GetComponent<CharacterController>();
            characterController.enabled = true;

			// Grab Move and Combat scripts for player
            move = GetComponent<MoveScript>();
            combat = GetComponent<CombatScript>();

            forwardSpeed = 0;
            sideSpeed = 0;
        }
    }

    // Handle player inputs and calculate movement variables
    void Update()
    {
        if (isLocalPlayer)
        {
            // Movement
             forwardSpeed = Input.GetAxis("Vertical");
             sideSpeed = Input.GetAxis("Horizontal");

            // Jump
			if (Input.GetButtonDown ("Jump") && characterController.isGrounded) {
				jump = true;
			} else {
				jump = false;
			}
        }
    }

	// Rotate and Move in fixed intervals
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
			// Rotation
			leftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
			verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

            forwardSpeed = forwardSpeed * movementSpeed * Time.fixedDeltaTime;
            sideSpeed = sideSpeed * movementSpeed * Time.fixedDeltaTime;

			//Jump
			if (jump) {
				verticalVelocity = jumpSpeed;
			}
			else if (characterController.isGrounded) {
				// This accounts for the slight upward shift that occurs when the collision system moves the two 
				// colliders apart. Otherwise isGrounded returns false sometimes when the character is just standing.
				verticalVelocity = reallySmallNumber;
			}
			else {
				verticalVelocity += gravity * Time.fixedDeltaTime * 0.5f;
			}

			speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
			speed = transform.rotation * speed;

			move.MoveCharacter(characterController, speed);
            move.RotateCharacter(verticalRotation, leftRight);
        }
    }

	// Lock the cursor to the center of the game window
    void SetCursorState()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
}
