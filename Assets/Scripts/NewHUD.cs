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
        Button joinButton;
        Button startButton;
        Button createMatchButton;
        InputField matchName;
        public GameObject startOnly;
        public GameObject startjoin;

        bool drawButtons = false;

        // Runtime variable
        bool m_ShowServer;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            mainCanvas = GameObject.Find ("MainCanvas").GetComponent<Canvas> ();
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
            startjoin.SetActive (false);
            manager.matchMaker.ListMatches (0, 20, "", manager.OnMatchList);
            drawButtons = true;
        }

        void StartButtonOnClick() {
            startOnly.SetActive(true);
            startjoin.SetActive (false);

            createMatchButton = GameObject.Find ("CreateMatchButton").GetComponent<Button> ();
            matchName = GameObject.Find ("NameInput").GetComponent<InputField> ();
            createMatchButton.onClick.AddListener ( () => { CreateMatchOnClick(); } );
        }

        void CreateMatchOnClick() {
            // Show stage selection menu
            mainMenuCamera.enabled = false;
            sceneSelectionCamera.enabled = true;
            startjoin.SetActive (false);
            startOnly.SetActive (false);
        }
            
        public void createMatch() {
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


        void Update()
        {
            print ("num players" + ClientScene.localPlayers.Count);
        }


        void OnGUI()
        {
            if (manager.matchMaker != null && drawButtons && manager.matches != null) {
                GUILayout.BeginArea (new Rect(Screen.width * (0.4f),
                    Screen.height * (0.75f), 
                    Screen.width * (0.2f), 
                    Screen.height * (0.2f)));
                foreach (var match in manager.matches) {
                    if (GUILayout.Button (match.name)) {
                        drawButtons = false;

                        manager.matchName = match.name;
                        manager.matchSize = (uint)match.currentSize;
                        manager.matchMaker.JoinMatch (match.networkId, "", manager.OnMatchJoined);
                    }
                }
                GUILayout.EndArea ();
            } else if (drawButtons && manager.matches == null) {
                GUIStyle style = new GUIStyle ();
                style.fontSize = 20;
                style.normal.textColor = Color.white;
                GUI.Label (new Rect(Screen.width * (0.4f),
                                    Screen.height * (0.75f), 
                                    Screen.width * (0.2f), 
                                    Screen.height * (0.2f)), 
                           "No Matches Found", style);
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