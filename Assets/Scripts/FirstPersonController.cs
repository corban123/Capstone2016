﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : NetworkBehaviour 
{

    public float movementSpeed = 10.0f;
    public float mouseSensitivity = 3.0f;
    public float jumpSpeed = 7.0f;
    float leftRight;
    private float nextFire;
    public float fireRate;

    [SerializeField] Camera FPSCam;
    [SerializeField] AudioListener audioListen;

    float verticalRotation = 0;
    public float upDownRange = 60.0f;

    float gravity = Physics.gravity.y / 2;
    float verticalVelocity;

    Vector3 speed = Vector3.zero;

    CharacterController characterController;
    PlayerScript fpController;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            leftRight = 0;
            characterController = GetComponent<CharacterController>();
            characterController.enabled = true;
            fpController = GetComponent<PlayerScript>();
            fpController.enabled = true;
            verticalVelocity = 0;
            FPSCam.enabled = true;
            audioListen.enabled = true;
        }
    }

    // Update is called once per frame
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

            // Could add in double jump here
            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                verticalVelocity = jumpSpeed;
            }
            if (Input.GetButtonDown("Fire1") && Time.time > nextFire) //PC control
            {
                nextFire = Time.time + fireRate;
                fpController.Shoot();

            }
            verticalVelocity += gravity * Time.deltaTime;

            speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
            speed = transform.rotation * speed * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            fpController.RotateCharacter(verticalRotation, leftRight);
            fpController.MoveCharacter(characterController, speed);
        }
    }
}
