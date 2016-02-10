using UnityEngine;
using System.Collections;

public class CombatScript : MonoBehaviour
{
    public GameObject shot;
    Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    public int numQuarks;
    public int numElements;

    GameObject heldElement; //This GameObject represents the current element held by the player, if the player is not holding an element set this value to null

    // Use this for initialization
    void Start () {
        heldElement = null;
        //numQuarks = 0;
        //numElements = 0;
        shotSpawn = gameObject.transform.GetChild(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && numQuarks > 0) //PC control
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    public void Shoot()
    {
        numQuarks--;
        Instantiate(shot, shotSpawn.position, Camera.main.transform.rotation);
    }
}
