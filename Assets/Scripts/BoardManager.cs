using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * Class for sending board information over the network.
 */
public class BoardManager : NetworkBehaviour {
	BoardGenerator generator;
	List<int[,]> boards;
	bool found;

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

			if (boardObjects.Length == 1) {
				boardObjects [0].SetBoard (boards [0]);
			} else if (boardObjects.Length == 2) {
				boardObjects [0].SetBoard (boards [0]);
				boardObjects [1].SetBoard (boards [1]);
				found = true;
			}
		}
	}
}
