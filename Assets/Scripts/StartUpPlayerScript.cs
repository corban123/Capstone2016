using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * Class for finding players when they are spawned and assigning values to them.
 */
public class StartUpPlayerScript : NetworkBehaviour {
	BoardGenerator generator;
	List<int[,]> boards;
	bool found;
    public GameObject Player2Prefab;
    private readonly int PLAYER1_LAYER = ~(1 << 9);
    private readonly int PLAYER2_LAYER = ~(1 << 8);

	void Start () {
		// Generate the boards
		generator = GetComponent<BoardGenerator> ();
		boards = generator.CreateBoards();
		found = false;
	}

	// Search for players and set bingo boards
	void Update () {
		if (!found) {
			BoardScript[] boardObjects = FindObjectsOfType (typeof(BoardScript)) as BoardScript[];
            if (boardObjects.Length == 2) {
				if (!boardObjects [0].gameObject.name.Contains ("female")) {
					boardObjects[0].gameObject.name = "Player 1";
					boardObjects[1].gameObject.name = "Player 2";
                    boardObjects[0].SetBoard(boards[0]);
                    boardObjects[1].SetBoard(boards[1]);

                    boardObjects [0].gameObject.GetComponentInChildren<Camera> ().cullingMask = PLAYER1_LAYER;
                    boardObjects [1].gameObject.GetComponentInChildren<Camera> ().cullingMask = PLAYER2_LAYER;

				} else {
                    boardObjects[1].gameObject.name = "Player 1";
					boardObjects[0].gameObject.name = "Player 2";
                    boardObjects[0].SetBoard(boards[1]);
                    boardObjects[1].SetBoard(boards[0]);	

                    boardObjects [1].gameObject.GetComponentInChildren<Camera> ().cullingMask = PLAYER1_LAYER;
                    boardObjects [0].gameObject.GetComponentInChildren<Camera> ().cullingMask = PLAYER2_LAYER;
				}
                found = true;
			}
		}
	}
}
