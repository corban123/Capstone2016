using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class sendBoard : NetworkBehaviour {
	SyncListInt boards = new SyncListInt();
	[SyncVar] int i;
	[SyncVar] int j;

	// Use this for initialization
	void OnEnable () {
		i = 0;
		j = 0;
		TransmitBoard ();
		SetEnemyBoard ();
	}

	void Awake(){
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetEnemyBoard(){
		if(!isLocalPlayer){
			int[,] setBoard = new int[4, 4];
			for (i = 0; i < 4; i++) {
				for (j = 0; j < 4; j++) {
					(setBoard [i, j] ) = boards[i+(j*4)];

				}
			}
			gameObject.GetComponent<BoardScript> ().SetEnemyBoard( setBoard);
		}
	}

	[Command]
	void CmdProvideBoardToServer(int[,] board){
		for (i = 0; i < 4; i++) {
			for (j = 0; j < 4; j++) {
				boards.Add (board [i, j]);
			
			}
		}
	}

	[ClientCallback]
	void TransmitBoard(){
		if (isLocalPlayer) {
			CmdProvideBoardToServer (this.gameObject.GetComponent<BoardScript>().board);
		}
	}
}
