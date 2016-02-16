using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardManager : NetworkBehaviour {
	private bool IS_DEBUG = true;

	BoardGenerator generator;

	Board board1;
	Board board2;

	// Use this for initialization
	void Start () {
		generator = GetComponent<BoardGenerator> ();
		List<int[,]> boards = generator.CreateBoards();
		board1 = new Board (boards.ElementAt(0));
		board2 = new Board (boards.ElementAt(1));

		if (IS_DEBUG) {
			board1.printBoard ();
			board2.printBoard ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public class Board {
		public int[,] board;
		public bool[,] scored;

		// Constructor takes a 2d array of elements
		public Board (int[,] board) {
			this.board = board;
		}

		// Mark an element as scored on this board
		// Also returns whether or not the score causes this board to win
		public bool score (int element) {
			// Find the element on the board

			// Mark the element as scored in the scored bitmap

			// Check whether the board is a winner

			return false;
		}

		// Print out board for debugging
		// Note that when this is printed only first two lines are seen
		// unless you click on the message
		public void printBoard () {
			print(ToString());
		}

		// @Override
		public string toString () {
			string result = "";
			for (int i = 0; i < 4; i++) {
				for (int j = 0; j < 4; j++) {
					// Add extra space for alignment
					if (board [i, j] < 10) {
						result += "  ";
					}

					result += (board [i, j] + "  ");
				}
				result += "\n";
			}
			return result;
		}
	}
}
