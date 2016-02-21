using UnityEngine;
using System.Collections;

/*
*   This script contains the logic for elements and their abilities/power ups
*/

public class ElementScript : MonoBehaviour
{
    public int cost;
    public int elementID;   //This id is unique to each element (numbered 1-16)
    public int carrier;     //This number is 1 or 2 depending on which player is holding it, or -1 depending on if nobody is holding it
    enum Element { Alkaline, Metals, Gases, Noble}
	// Use this for initialization
	void Start ()
    {
        carrier = -1;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    Element IsElementType() //Useful to know what kind of element this script is attached to
    {
        if(elementID >= 0 && elementID <= 3)
        {
            return Element.Alkaline;
        }
        else if (elementID >= 4 && elementID <= 7)
        {
            return Element.Metals;
        }
        else if (elementID >= 8 && elementID <= 11)
        {
            return Element.Gases;
        }
        else
        {
            return Element.Noble;
        }
    }

}
