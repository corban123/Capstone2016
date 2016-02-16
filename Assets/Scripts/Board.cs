using UnityEngine;
using System.Collections;

/*
 * Board object for storing board, scoring, and printing.
 */
public class Board {
	private int[,] board;
	private bool[,] scored;
	private string boardText;

	// Constructor takes a 2d array of elements
	public Board (int[,] board) {
		this.board = board;
		this.scored = new bool[4, 4];
		this.boardText = ToString();
	}

	// Marks an element as scored on this board
	// Returns whether or not the score causes this board to win.
	public bool score (int element) {
		// Find the element on the board

		// Mark the element as scored in the scored bitmap

		// Change Board text

		// Check whether the board is a winner

		return false;
	}

	// @Override prints board results.
	// includes '-' if element has been collected.
	public override string ToString () {
		string result = "";
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				if (scored[i, j]) {
					result += "  -  ";
				} else {
					// Add extra space for alignment
					if (board [i, j] < 10) {
						result += "  ";
					}
					result += (board [i, j] + "  ");
				}
			}
			result += "\n";
		}
		return result;
	}

	public string getBoardText() {
		return boardText;
	}
}
