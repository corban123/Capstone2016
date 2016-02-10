using UnityEngine;
using System.Collections;

public class CombatScript : MonoBehaviour {
    public GameObject shot;
    Transform shotSpawn;
    private float nextFire;
    public float fireRate;
    MoveScript player;

    // Use this for initialization

    void Start () {
        shotSpawn = gameObject.transform.GetChild(0);
        player = gameObject.GetComponent<MoveScript>();
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire) //PC control
        //if (Input.GetButtonDown("Fire1") && Time.time > nextFire && player.numQuarks > 0) //PC control
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    public void Shoot()
    {
        player.numQuarks--;
        Instantiate(shot, shotSpawn.position, Camera.main.transform.rotation);
    }
}
