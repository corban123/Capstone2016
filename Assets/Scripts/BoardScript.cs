using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

/*
 * Board object for storing board, scoring, and printing.
 */
public class BoardScript : NetworkBehaviour {
	private int[,] board;
	private bool[,] scored;
	private Text boardText;

	void Start() {
		scored = new bool[4, 4];
		boardText = GameObject.Find("BoardText").GetComponent<Text>();
	}


	// Set the board value
	public void SetBoard (int[,] board) {
		this.board = board;
		SetBoardText ();
	}

	// Marks an element as scored on this board
	// Returns whether or not the score causes this board to win.
	public bool score (int element) {
		// Find the element on the board
		int[] coordinates = GetCoordinates(element);

		// Mark the element as scored in the scored bitmap
		scored[coordinates[0], coordinates[1]] = true;

		// Change Board text
		SetBoardText();

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

	// Get coordinates of a value in the board
	private int[] GetCoordinates(int val) {
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				if (board[i,j] == val) {
					int[] result = { i, j };
					return result;
				}
			}
		}
		return null;
	}

	private void SetBoardText() {
		if (isLocalPlayer)
		{
			boardText.text = "Bingo Board\n" + ToString();
		}
	}
}
