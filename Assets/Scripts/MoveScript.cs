using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour
{
    CombatScript combat;

	// Use this for initialization
	void Start ()
    {
        combat = gameObject.GetComponent<CombatScript>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        
    }

    public void RotateCharacter(float verticalRotation, float leftRight)
    {
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(0, leftRight, 0);
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
            combat.numElements += 1;
        }
        if (collision.tag == "Quark")
        {
            print("picked up quark");
            Destroy(collision.gameObject);
            //collision.gameObject.SendMessage("PickUp");
            combat.numQuarks += 1;
        }
    }
}
