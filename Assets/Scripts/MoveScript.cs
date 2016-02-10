using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {
    public int numElements;
    public int numQuarks;
    public GameObject shot;
    Transform shotSpawn;

	// Use this for initialization
	void Start () {
        shotSpawn = this.gameObject.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {

        
    }

    public void RotateCharacter(float verticalRotation, float leftRight)
    {
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(0, leftRight, 0);

    }
    public void Shoot()
    {
        Instantiate(shot, shotSpawn.position, gameObject.transform.rotation);
    }

    public void MoveCharacter(CharacterController characterController, Vector3 speed)
    {
        characterController.Move(speed);
    }

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
