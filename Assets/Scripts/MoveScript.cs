using UnityEngine;
using System.Collections;

/*
 * A script for moving the character and rotating the FPC camera.
 */
public class MoveScript : MonoBehaviour
{
    public int numElements;
    public int numQuarks;
	// TODO(@josh): Move to CombatScript
    public GameObject shot;
    Transform shotSpawn;

	// Use this for initialization
	void Start () 
	{
		// TODO(@josh): move to CombatScript
        shotSpawn = this.gameObject.transform.GetChild(0);
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

	// TODO(@josh): move to ProjectileScript(?)
    public void Shoot()
    {
        Instantiate(shot, shotSpawn.position, gameObject.transform.rotation);
    }

	// Move the given CharacterController by the vector speed.
    public void MoveCharacter(CharacterController characterController, Vector3 speed)
    {
        characterController.Move(speed);
    }

	// Pick up elements and quarks on collision
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Element")
        {
            print("picked up element");
            Destroy(collision.gameObject);
            //collision.gameObject.SendMessage("PickUp");
            numElements += 1;
        }
        if (collision.tag == "Quark")
        {
            print("picked up quark");
            Destroy(collision.gameObject);
            //collision.gameObject.SendMessage("PickUp");
            numQuarks += 1;
        }
    }


    //void OnControllerColliderHit(ControllerColliderHit collision)
    //{
    //    print("hello");
    //    if (collision.collider.CompareTag("Element"))
    //    {
    //        print("collision with element");
    //        Destroy(collision.gameObject);
    //        //collision.gameObject.SendMessage("PickUp");
    //        numElements += 1;
    //    }
    //    if (collision.collider.CompareTag("Quark"))
    //    {
    //        print("collision with quark");
    //        Destroy(collision.gameObject);
    //        //collision.gameObject.SendMessage("PickUp");
    //        numQuarks += 1;
    //    }
    //}
}
