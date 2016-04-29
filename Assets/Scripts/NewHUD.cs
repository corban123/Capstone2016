using System;
using System.ComponentModel;
using UnityEngine.UI;

#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class NewHUD : MonoBehaviour
    {
        public NetworkManager manager;
        [SerializeField] public bool showGUI = false;
        [SerializeField] public int offsetX;
        [SerializeField] public int offsetY;

        public Camera mainMenuCamera;
        public Camera sceneSelectionCamera;

        Canvas mainCanvas;
        Canvas arenaSelectionCanvas;
        Canvas loadingCanvas;
        Button joinButton;
        Button backJoinButton;
        Button startButton;
        Button backCreateMatchButton;
        Button createMatchButton;
        InputField matchName;
        public GameObject startOnly;
        public GameObject startjoin;
        public GameObject joinOnly;

        bool drawButtons = false;

        // Runtime variable
        bool m_ShowServer;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            mainCanvas = GameObject.Find ("MainCanvas").GetComponent<Canvas> ();
            loadingCanvas = GameObject.Find ("LoadingCanvas").GetComponent<Canvas> ();
            arenaSelectionCanvas = GameObject.Find ("ArenaSelectionCanvas").GetComponent<Canvas> ();
            joinButton = GameObject.Find ("JoinGameButton").GetComponent<Button> ();
            startButton = GameObject.Find ("StartGameButton").GetComponent<Button> ();


            enableMatchMaker ();
            joinButton.onClick.AddListener ( () => { JoinButtonOnClick(); } );
            startButton.onClick.AddListener ( () => { StartButtonOnClick(); } );
        }

        void enableMatchMaker() {
            bool noConnection = (manager.client == null || manager.client.connection == null ||
                manager.client.connection.connectionId == -1);
            
            if (manager.matchMaker == null && !NetworkServer.active && !manager.IsClientConnected() && noConnection)
            {
                manager.StartMatchMaker();
            }
        }

        void JoinButtonOnClick() {
            GoToJoinOnly ();

            backJoinButton = GameObject.Find ("BackJoinButton").GetComponent<Button> ();
            backJoinButton.onClick.AddListener ( () => { BackJoinOnClick(); } );
            manager.matchMaker.ListMatches (0, 20, "", manager.OnMatchList);
        }

        void BackJoinOnClick() {
            GoToStartJoin ();
        }
            

        void StartButtonOnClick() {
            GoToStartOnly ();

            backCreateMatchButton = GameObject.Find ("BackCreateMatchButton").GetComponent<Button> ();
            createMatchButton = GameObject.Find ("CreateMatchButton").GetComponent<Button> ();
            matchName = GameObject.Find ("NameInput").GetComponent<InputField> ();
            backCreateMatchButton.onClick.AddListener ( () => { BackCreateMatchOnClick(); } );
            createMatchButton.onClick.AddListener ( () => { CreateMatchOnClick(); } );
        }

        void BackCreateMatchOnClick() {
            GoToStartJoin ();
        }

        void CreateMatchOnClick() {
            // Show stage selection menu
            GoToArenaSelectionScreen();
        }
            
        public void createMatch() {
            GoToLoadingScreen ();
            
            if (manager.matchMaker != null && manager.matchInfo == null && manager.matches == null) {
                manager.matchName = matchName.text;
                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
            }
            if (manager.IsClientConnected () && !ClientScene.ready) {
                ClientScene.Ready (manager.client.connection);

                if (ClientScene.localPlayers.Count == 0) {
                    ClientScene.AddPlayer (0);
                }
            }
        }

        void GoToStartJoin() {
            startjoin.SetActive (true);
            startOnly.SetActive (false);
            joinOnly.SetActive (false);

            drawButtons = false;
        }

        void GoToStartOnly() {
            startjoin.SetActive (false);
            startOnly.SetActive (true);
            joinOnly.SetActive (false);

            drawButtons = false;
        }

        void GoToJoinOnly() {
            startjoin.SetActive (false);
            startOnly.SetActive (false);
            joinOnly.SetActive (true);

            drawButtons = true;
        }

        void GoToLoadingScreen() {
            startjoin.SetActive (false);
            startOnly.SetActive (false);
            joinOnly.SetActive (false);

            mainCanvas.enabled = false;
            arenaSelectionCanvas.enabled = false;
            loadingCanvas.enabled = true;

            mainMenuCamera.enabled = false;
            sceneSelectionCamera.enabled = false;
        }

        public void GoToMainScreen() {
            GoToStartJoin ();

            mainCanvas.enabled = true;
            arenaSelectionCanvas.enabled = false;
            loadingCanvas.enabled = false;

            mainMenuCamera.enabled = true;
            sceneSelectionCamera.enabled = false;
        }

        void GoToArenaSelectionScreen() {
            startjoin.SetActive (false);
            startOnly.SetActive (false);
            joinOnly.SetActive (false);

            mainCanvas.enabled = false;
            arenaSelectionCanvas.enabled = true;
            loadingCanvas.enabled = false;

            mainMenuCamera.enabled = false;
            sceneSelectionCamera.enabled = true;
        }


        void Update()
        {
        }


        void OnGUI()
        {
            GUIStyle style = new GUIStyle ();
            style.fontSize = 20;
            style.normal.textColor = Color.white;

            if (drawButtons && manager.matches != null && manager.matchMaker != null) {
                if (manager.matches.Count == 0) {
                    GUI.Label (new Rect (Screen.width * (0.4f),
                        Screen.height * (0.75f), 
                        Screen.width * (0.2f), 
                        Screen.height * (0.2f)), 
                        "No Matches Found", style);
                } else {
                    GUILayout.BeginArea (new Rect (Screen.width * (0.4f),
                        Screen.height * (0.75f), 
                        Screen.width * (0.2f), 
                        Screen.height * (0.2f)));
                    foreach (var match in manager.matches) {
                        if (GUILayout.Button (match.name)) {
                            drawButtons = false;
                            GoToLoadingScreen ();

                            manager.matchName = match.name;
                            manager.matchSize = (uint)match.currentSize;
                            manager.matchMaker.JoinMatch (match.networkId, "", manager.OnMatchJoined);
                        }
                    }
                    GUILayout.EndArea ();
                }
            } else if (manager.matchMaker == null) {
                GUI.Label (new Rect (Screen.width * (0.4f),
                    Screen.height * (0.75f), 
                    Screen.width * (0.2f), 
                    Screen.height * (0.2f)), 
                    "Match Maker Error", style);
                
            } else if (drawButtons) {
                GUI.Label (new Rect (Screen.width * (0.4f),
                    Screen.height * (0.75f), 
                    Screen.width * (0.2f), 
                    Screen.height * (0.2f)), 
                    "Finding Matches...", style);
            }



            if (!showGUI)
                return;

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            const int spacing = 24;

            bool noConnection = (manager.client == null || manager.client.connection == null ||
                manager.client.connection.connectionId == -1);

            if (manager.IsClientConnected() && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
                {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || manager.IsClientConnected())
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
                {
                    manager.StopHost();
                }
                ypos += spacing;
            }

            if (!NetworkServer.active && !manager.IsClientConnected() && noConnection)
            {
                ypos += 10;

                if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    GUI.Box(new Rect(xpos - 5, ypos, 220, 25), "(WebGL cannot use Match Maker)");
                    return;
                }

                if (manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
                    {
                        manager.StartMatchMaker();
                    }
                    ypos += spacing;
                }
                else
                {
                    if (manager.matchInfo == null)
                    {
                        if (manager.matches == null)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                            {
                                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
                            }
                            ypos += spacing;

                            GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                            manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), manager.matchName);
                            ypos += spacing;

                            ypos += 10;

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                            {
                                manager.matchMaker.ListMatches(0, 20, "", manager.OnMatchList);
                            }
                            ypos += spacing;
                        }
                        else
                        {
                            foreach (var match in manager.matches)
                            {
                                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                                {
                                    manager.matchName = match.name;
                                    manager.matchSize = (uint)match.currentSize;
                                    manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
                                }
                                ypos += spacing;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
                    {
                        m_ShowServer = !m_ShowServer;
                    }
                    if (m_ShowServer)
                    {
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
                        {
                            manager.SetMatchHost("localhost", 1337, false);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                        {
                            manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                        {
                            manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                    }

                    ypos += spacing;

                    GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                    {
                        manager.StopMatchMaker();
                    }
                    ypos += spacing;
                }
            }
        }
    }
}
#endif //ENABLE_UNET