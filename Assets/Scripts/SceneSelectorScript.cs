using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SceneSelectorScript : MonoBehaviour {
    NetworkManager manager;
    public Object arenaOne;
    public Object arenaTwo;
    public Object arenaThree;
    public Object arenaFour;


	// Use this for initialization
	void Start () {
        manager = GameObject.Find("NetManager").GetComponent<NetworkManager>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp() {
        manager.GetComponent<startCreatePlayers>().onlineScene = getScene(gameObject.tag);
        manager.GetComponent<NewHUD>().createMatch();
    }

    string getScene(string tag) {
        if (tag == "Arena1") {
            return arenaOne.name;
        } else if (tag == "Arena2") {
            return arenaTwo.name;
        } else if (tag == "Arena3") {
            return arenaThree.name;
        } else if (tag == "Arena4") {
            return arenaFour.name;
        } else {
            return null;
        }
    }
}
