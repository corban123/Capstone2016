using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class MetallizeScript : NetworkBehaviour {

    float range = 20f;

    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 3f);
        foreach (Collider collider in Physics.OverlapSphere(transform.position, range))
        {
            FirstPersonController fpc = collider.GetComponent<FirstPersonController>();
            if (fpc != null)
            {
                print("Metallizing " + fpc.gameObject.name);
                fpc.FreezeMovement();
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {       
        CmdRemoveMetal();
    }

    [Command]
    void CmdRemoveMetal()
    {
        NetworkServer.Destroy(this.gameObject);
    }

}
