using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SceneSelectorScript : MonoBehaviour {
    NetworkManager manager;
    NewHUD hud;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("NetManager").GetComponent<NetworkManager>();
        hud = manager.GetComponent<NewHUD> ();

        GameObject.Find("Arena1").GetComponent<Button>().onClick.AddListener( () => {onClickArena("Noah-Lv1"); } );
        GameObject.Find("Arena2").GetComponent<Button>().onClick.AddListener( () => {onClickArena("Mitchell-LevelOne"); } );
        GameObject.Find("Arena3").GetComponent<Button>().onClick.AddListener( () => {onClickArena("Noah-Lv2"); } );
        GameObject.Find("Arena4").GetComponent<Button>().onClick.AddListener( () => {onClickArena("Mitchell-LevelTwo"); } );

    }

    // Update is called once per frame
    void Update () {

    }

    public void onClickArena(string sceneName) {
        manager.GetComponent<startCreatePlayers>().onlineScene = sceneName;

        hud.createMatch ();
    }
}
