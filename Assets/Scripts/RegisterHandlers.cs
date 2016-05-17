using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RegisterHandlers : NetworkBehaviour {
	private NetworkClient client;

	public const short RegisterBoardHostMsgId = 888;
	public const short RegisterHitHostMsgId = 900;

	public const short RegisterBoardClientMsgId = 999;
	public const short RegisterHitClientMsgId = 1000;


	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			client = NetworkClient.allClients [0];

			NetworkServer.RegisterHandler(RegisterBoardHostMsgId, GetComponent<BoardScript>().SetEnemyBoard);
			NetworkServer.RegisterHandler(RegisterBoardHostMsgId+1 , GetComponent<BoardScript>().stopSendingBoard);

			NetworkServer.RegisterHandler(RegisterHitHostMsgId , GetComponent<CombatScript>().ThrowDamageSound);
			NetworkServer.RegisterHandler(RegisterHitClientMsgId , GetComponent<CombatScript>().ThrowDamageSound);


			client.RegisterHandler(RegisterBoardClientMsgId, GetComponent<BoardScript>().SetEnemyBoard);
			client.RegisterHandler (RegisterHitClientMsgId, GetComponent<CombatScript> ().ThrowDamageSound);
			client.RegisterHandler (RegisterHitHostMsgId, GetComponent<CombatScript> ().ThrowDamageSound);



		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
