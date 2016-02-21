using UnityEngine;
using System.Collections;

/*
*   This script controls the behavior of each player's base, such as when they try to turn in an element
*/
public class BaseScript : MonoBehaviour {
    int BaseId; //This number represents which player the base responds to (player 1 or 2)
    
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Element")
        {
            int element = coll.gameObject.GetComponent<ElementScript>().elementID;
            int carrier = coll.gameObject.GetComponent<ElementScript>().carrier;
            if (carrier == BaseId)
            {
                GameObject player = GameObject.Find("Player " + carrier); //Assuming that a player is named Player <number> i.e Player 1
                player.GetComponent<BoardScript>().score(element);
            }
        }

    }

}
