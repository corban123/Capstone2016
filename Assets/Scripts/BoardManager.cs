using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * Class for sending board information over the network.
 */
public class BoardManager : NetworkBehaviour {
	private bool IS_DEBUG = true;

	BoardGenerator generator;

	Board board1;
	Board board2;

	void Start () {
		// Generate the boards
		generator = GetComponent<BoardGenerator> ();
		List<int[,]> boards = generator.CreateBoards();
		board1 = new Board (boards.ElementAt(0));
		board2 = new Board (boards.ElementAt(1));

		if (IS_DEBUG) {
			print (board1.getBoardText ());
			print (board2.getBoardText ());
		}
	}
	
	// Send a board to each player
		
}
