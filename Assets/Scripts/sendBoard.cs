using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class sendBoard : NetworkBehaviour {
    SyncListInt boards = new SyncListInt();
    public int[] enemyBoard = new int[16];
    [SyncVar]
    int testInt;
	[SyncVar] int i;
	[SyncVar] int j;

	// Use this for initialization
	void OnEnable () {
        TransmitBoard();
		i = 0;
		j = 0;
		
	}

	void Awake(){
		
	
	}
	
 [ClientRpc]
	public void RpcSetEnemyBoard(){
		if(!isLocalPlayer){

            for(int i = 0; i < enemyBoard.Length; i++)
            {
                enemyBoard[i] = boards[i];

            }
            
		}
	}

	[Command]
	void CmdProvideBoardToServer(int[] board, int x){
            for (i = 0; i < 16; i++)
            {
                
                    boards.Add(board[i]);
                    testInt = 6;

                
            }
        RpcSetEnemyBoard();        
	}


    public void TransmitBoard()
    {
        if (isLocalPlayer)
        {
            if (isServer)
            {

                gameObject.GetComponent<BoardScript>().HostSendBoard();

            }
            else if (isClient)
            {
                gameObject.GetComponent<BoardScript>().ClientSendBoard();
            }
        }
    }
}
