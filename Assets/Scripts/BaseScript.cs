using UnityEngine;
using System.Collections;

/*
*   This script controls the behavior of each player's base, such as when they try to turn in an element
*/
public class BaseScript : MonoBehaviour {
    public int BaseId; //This number represents which player the base responds to (player 1 or 2)
    
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.name.Contains("Element"))
        {
            int element = coll.gameObject.GetComponent<ElementScript>().elementID;
            int carrier = coll.gameObject.GetComponent<ElementScript>().carrier;
            print("Element has hit a base from player" + carrier);
            if (carrier == BaseId)
            {
                print("Finding Player " + carrier);
                GameObject player = GameObject.Find("Player " + carrier); //Assuming that a player is named Player <number> i.e Player 1
                print("Player " + carrier + " scored!");
                player.GetComponent<BoardScript>().score(element);
                player.GetComponent<GUIScript> ().enableYouScored ();
            }
        }

    }

}
