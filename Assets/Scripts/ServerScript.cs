using UnityEngine;
using System.Collections;

public class ServerScript : MonoBehaviour {

    public Object serverPrefab;
    public Transform serverStart;

    // Use this for initialization
    void Awake()
    {
        Network.InitializeServer(128, 45749, true);
        MasterServer.RegisterHost("TestBuild", "test");
        GameObject player = ((GameObject)Network.Instantiate(serverPrefab, serverStart.position, serverStart.rotation, 0));
        Debug.Log("Server");
    }


    // Update is called once per frame
    void Update()
    {

    }
}