using UnityEngine;
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

    public float movementSpeed = 50.0f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 10.0f;
    float leftRight;

    private float nextFire;
    public float fireRate;

    [SerializeField] Camera FPSCam;
    [SerializeField] AudioListener audioListen;

    float verticalRotation = 0;
    public float upDownRange = 60.0f;

    float gravity = Physics.gravity.y / 1.5f;
    float verticalVelocity;

    Vector3 speed = Vector3.zero;

    CharacterController characterController;
    MoveScript move;

    // Initalize variables for this character
    void Start()
    {
        if (isLocalPlayer)
        {
			// Initalize variables
            leftRight = 0;
			verticalVelocity = 0;

			// Enable the character controller for this player
            characterController = GetComponent<CharacterController>();
            characterController.enabled = true;

			// Enable the move script for this player
            move = GetComponent<MoveScript>();
            move.enabled = true;
            
			// Enable the camera and audio for this player
            FPSCam.enabled = true;
            audioListen.enabled = true;
        }
    }

    // Handle player inputs and calculate movement variables
    void Update()
    {
        if (isLocalPlayer)
        {
            // Rotation
            leftRight = Input.GetAxis("Mouse X") * mouseSensitivity;

            verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

            // Movement
            float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
            float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

            // Jump
            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                verticalVelocity = jumpSpeed;
            }

			verticalVelocity += gravity * Time.deltaTime;

			// TODO(@josh): move to CombatScript
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire) //PC control
            {
                nextFire = Time.time + fireRate;
                move.Shoot();
            }

            speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
            speed = transform.rotation * speed * Time.deltaTime;
        }
    }

	// Rotate and Move in fixed intervals
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            move.RotateCharacter(verticalRotation, leftRight);
            move.MoveCharacter(characterController, speed);
        }
    }
}
