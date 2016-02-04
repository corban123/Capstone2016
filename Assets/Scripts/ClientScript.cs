using UnityEngine;
using System.Collections;

public class ClientScript : MonoBehaviour {

    private bool connected = false;
    public Object clientPrefab;
    public Transform clientStart;


    // Use this for initialization
    void Awake()
    {
        MasterServer.RequestHostList("TestBuild");
    }

    void OnConnectedToServer()
    {
        ((GameObject)Network.Instantiate(clientPrefab, clientStart.position, clientStart.rotation, 0)).GetComponentInChildren<Camera>().enabled = true;
        Debug.Log("yeah");
    }

    // Update is called once per frame
    void Update()
    {
        if (!connected)
        {
            Debug.Log("Client");
            HostData[] hosts = MasterServer.PollHostList();
            if (hosts.Length > 0)
            {
                Network.Connect(hosts[0]);
                connected = true;
            }
        }

    }
}