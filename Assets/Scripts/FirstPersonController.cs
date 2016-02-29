using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/*
 * A class for handling player inputs and movement calculations.
 * 
 * Also handles death from falling off level.
 */
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : NetworkBehaviour 
{
	[SerializeField] Camera FPSCam;
	[SerializeField] AudioListener audioListen;
			 private AudioSource source;
	[SerializeField] AudioClip jumpUp;
	[SerializeField] AudioClip jumpLand;

    public float movementSpeed = 25.0f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 8.0f;

	Vector3 moveDirection;
    float forwardSpeed;
    float sideSpeed;

	private float gravity = -20.0f;
	private bool jump = false;
    
	float leftRight;
	float verticalRotation = 0;
	float upDownRange = 60.0f;

    private float nextFire;
    public float fireRate;

    CharacterController characterController;
    MoveScript move;



    // Initalize variables for this character
    void Start()
    {
        if (isLocalPlayer)
        {
			//SetCursorState ();

			// Enable the character controller for this player
            characterController = GetComponent<CharacterController>();
            characterController.enabled = true;
			source = GetComponent<AudioSource> ();

			// Grab Move and Combat scripts for player
            move = GetComponent<MoveScript>();

            forwardSpeed = 0;
            sideSpeed = 0;
			leftRight = 0;
			moveDirection = Vector3.zero;
        }
    }

    // Handle player inputs and calculate movement variables
    void Update()
    {
        if (isLocalPlayer)
        {
            // Jump
			if (Input.GetButtonDown ("Jump") && !jump) {
				source.clip = jumpUp;
				source.Play ();
				jump = true;
			}
        }

    }

	// Rotate and Move in fixed intervals
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            forwardSpeed = Input.GetAxis("Vertical");
            sideSpeed = Input.GetAxis("Horizontal");
            // Rotation
			leftRight = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
			verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

			// Movement
			forwardSpeed = forwardSpeed * movementSpeed;
			sideSpeed = sideSpeed * movementSpeed;

			if (characterController.isGrounded) {
				// Reset y velocity if on the ground
				moveDirection = new Vector3 (sideSpeed, gravity, forwardSpeed);

				// Unless we are jumping
				if (jump) {
					moveDirection.y = jumpSpeed;
				}
			} else {
				moveDirection.x = sideSpeed;
				moveDirection.z = forwardSpeed;
                jump = false;
			}
                
			moveDirection = transform.TransformDirection (moveDirection);

			// Apply gravity
			moveDirection.y += gravity * Time.fixedDeltaTime;
            move.MoveCharacter(characterController, moveDirection * Time.fixedDeltaTime);

            move.RotateCharacter(verticalRotation , leftRight);
        }
    }

	// Lock the cursor to the center of the game window
    void SetCursorState()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
}
