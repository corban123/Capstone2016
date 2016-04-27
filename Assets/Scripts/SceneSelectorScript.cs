using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SceneSelectorScript : MonoBehaviour {
    NetworkManager manager;
    NewHUD hud;


	// Use this for initialization
	void Start () {
        manager = GameObject.Find("NetManager").GetComponent<NetworkManager>();
        hud = manager.GetComponent<NewHUD> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp() {
        string sceneName = getScene (gameObject.tag);
        manager.GetComponent<startCreatePlayers>().onlineScene = sceneName;

        hud.createMatch ();
    }

    string getScene(string tag) {
        if (tag.Equals("Arena1")) {
            return "Noah-Lv1";
        } else if (tag.Equals("Arena2")) {
            return "Mitchell-LevelOne";
        } else if (tag == "Arena3") {
            return "Noah-Lv2";
        } else if (tag == "Arena4") {
            return "Mitchell-LevelTwo";
        } else {
            return null;
        }
    }
}
