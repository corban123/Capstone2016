using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class MyController : MonoBehaviour {

	public float movementSpeed = 10.0f;
	public float mouseSensitivity = 3.0f;
	public float jumpSpeed = 7.0f;
	
	float verticalRotation = 0;
	public float upDownRange = 60.0f;
	
	float gravity = Physics.gravity.y / 2;
	float verticalVelocity;

	Vector3 speed = Vector3.zero;

	CharacterController characterController;
	FirstPersonController fpController;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
		fpController = GetComponent<FirstPersonController> ();
		verticalVelocity = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Rotation
		float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
		transform.Rotate (0, rotLeftRight, 0);
		
		verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);

		// Movement
		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;
		
		// Could add in double jump here
		if (Input.GetButtonDown ("Jump") && characterController.isGrounded) {
			verticalVelocity = jumpSpeed;
		}
			
		verticalVelocity += gravity * Time.deltaTime;

		speed = new Vector3 (sideSpeed, verticalVelocity, forwardSpeed);
		speed = transform.rotation * speed * Time.deltaTime;
	}

	void FixedUpdate () {
		fpController.RotateCharacter (verticalRotation);
		fpController.MoveCharacter (characterController, speed);
	}
}
