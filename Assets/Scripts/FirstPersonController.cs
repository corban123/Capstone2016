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

    public float movementSpeed = 25.0f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 8.0f;

	Vector3 moveDirection;
    float forwardSpeed;
    float sideSpeed;
	float verticalSpeed;

	private float reallySmallNumber = -0.001f;
	private float gravity = -15.0f;
	private bool jump = false;
    
	float leftRight;
	float verticalRotation = 0;
	float upDownRange = 60.0f;

    private float nextFire;
    public float fireRate;

    CharacterController characterController;
    MoveScript move;
    CombatScript combat;

    // Initalize variables for this character
    void Start()
    {
        if (isLocalPlayer)
        {
			//SetCursorState ();

			// Enable the character controller for this player
            characterController = GetComponent<CharacterController>();
            characterController.enabled = true;

			// Grab Move and Combat scripts for player
            move = GetComponent<MoveScript>();
            combat = GetComponent<CombatScript>();

            forwardSpeed = 0;
            sideSpeed = 0;
			verticalSpeed = 0;
			leftRight = 0;
			moveDirection = Vector3.zero;
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

			// Movement
			forwardSpeed = forwardSpeed * movementSpeed;
			sideSpeed = sideSpeed * movementSpeed;

			if (characterController.isGrounded) {
				// Reset y velocity if on the ground
				moveDirection = new Vector3 (sideSpeed, reallySmallNumber, forwardSpeed);

				// Unless we are jumping
				if (jump) {
					moveDirection.y = jumpSpeed;
				}
			} else {
				moveDirection.x = sideSpeed;
				moveDirection.z = forwardSpeed;
			}

			moveDirection = transform.TransformDirection (moveDirection);

			// Apply gravity
			moveDirection.y += gravity * Time.fixedDeltaTime;

			move.MoveCharacter(characterController, moveDirection * Time.fixedDeltaTime);
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
