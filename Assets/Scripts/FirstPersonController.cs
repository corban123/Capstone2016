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
	float upDownRange = 60.0f;

    float startTime;
    float duration = 2.0f;
    bool freezing;
    float freezeRate = 0.025f;

	Vector3 moveDirection;
    float forwardSpeed;
    float sideSpeed;
    float moveFactor;

	private float gravity = -20.0f;
	private bool jump = false;
    
	float leftRight;
	float verticalRotation = 0;

    private float nextFire;
    public float fireRate;

    CharacterController characterController;
    MoveScript move;



    // Initalize variables for this character
    void Start()
    {
        if (isLocalPlayer)
        {
			SetCursorState ();

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
            freezing = false;
            moveFactor = 1.0f;
        }
    }

    // Handle player inputs and calculate movement variables
    void Update()
    {
        if (isLocalPlayer)
        {
            // Jump
			if (Input.GetButtonDown ("Jump") && !jump){
			if(source.clip != jumpUp){
					source.loop = false;
					source.Stop ();
				}
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
			leftRight = Input.GetAxis("Mouse X") * mouseSensitivity * (Time.fixedDeltaTime* 10);
			verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
			float verticalRotation1 = verticalRotation;
			if (verticalRotation > upDownRange) {
				verticalRotation1 = upDownRange;
			
			} else if (verticalRotation < -upDownRange) {
				verticalRotation1 = -upDownRange;
			}

            move.RotateCharacter (verticalRotation1, leftRight);

			// Movement
            if (freezing) {
                // Unfreeze
                if (Time.time - startTime > duration) {
                    print ("Unfreezing movement");
                    moveFactor = 1.0f;
                    freezing = false;
                }
                // Frozen
                else if (moveFactor <= 0.0f) {
                    moveFactor = 0.0f;
                } 
                // Slowing down
                else {
                    moveFactor -= freezeRate;
                    print (moveFactor);
                }
            }

            forwardSpeed = forwardSpeed * movementSpeed * moveFactor;
            sideSpeed = sideSpeed * movementSpeed * moveFactor;

            if (characterController.isGrounded) {
                // Reset y velocity if on the ground
                moveDirection = new Vector3 (sideSpeed, gravity, forwardSpeed);

                // Unless we are jumping
                if (jump && !freezing) {
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
            move.MoveCharacter (characterController, moveDirection * Time.fixedDeltaTime);
    	}
	}

	// Lock the cursor to the center of the game window
    void SetCursorState()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    public void FreezeMovement()
    {
        if (isLocalPlayer)
        {
            startTime = Time.time;
            print ("Freezing movement at Time: " + startTime);
            freezing = true;
        }
    }
        
}
