using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

/*
 * Board object for storing board, scoring, and printing.
 */
public class BoardScript : NetworkBehaviour {
	private int[,] board;
	private bool[,] scored = new bool[4, 4];
    private GameObject boardUI;
    private bool UICreated = false;
    public GameObject barium;
    public GameObject calcium;
    public GameObject carbon;
    public GameObject copper;
    public GameObject gold;
    public GameObject helium;
    public GameObject hydrogen;
    public GameObject krypton;
    public GameObject neon;
    public GameObject nickel;
    public GameObject nitrogen;
    public GameObject oxygen;
    public GameObject potassium;
    public GameObject silver;
    public GameObject sodium;
    public GameObject xenon;

	void Start() {
	}


	// Set the board value
	public void SetBoard (int[,] board) {
        boardUI = GameObject.Find("BingoBoard");
		this.board = board;
        if (!UICreated) {
            CreateBingoBoardUI ();
            UICreated = true;
        }
	}

	// Marks an element as scored on this board
	// Returns whether or not the score causes this board to win.
	public bool score (int element) {
		// Find the element on the board
		int[] coordinates = GetCoordinates(element);

		// Mark the element as scored in the scored bitmap
		scored[coordinates[0], coordinates[1]] = true;

		// Change Board text
		//SetBoardText();

		// Check whether the board is a winner

		return isWin(coordinates[0], coordinates[1]);
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

	// Returns whether or not marking off the element at (x, y) causes a player to win.
	private bool isWin(int x, int y) {
		bool win_horizontal = true;
		bool win_vertical = true;
		bool win_diagonal_back = true;
		bool win_diagonal_forward = true;

		// Check for wins
		for (int i = 0; i < 4; i++) {
			win_horizontal = win_horizontal && scored [i, y];
			win_vertical = win_vertical && scored[x, i];
			win_diagonal_forward = win_diagonal_forward && scored [i, i];
			win_diagonal_back = win_diagonal_back && scored [i, 3 - i];
		}
				
		return win_horizontal || win_vertical || win_diagonal_back || win_diagonal_forward;
	}

    public int getValueAtPoint(int i, int j)
    {

        return board[i, j];
    }

    private void CreateBingoBoardUI (){
        barium = Instantiate (barium) as GameObject;
        barium.transform.SetParent(boardUI.transform, false);

        carbon = Instantiate (carbon) as GameObject;
        carbon.transform.SetParent(boardUI.transform, false);

        calcium = Instantiate (calcium) as GameObject;
        calcium.transform.SetParent(boardUI.transform, false);

        copper = Instantiate (copper) as GameObject;
        copper.transform.SetParent (boardUI.transform, false);

        gold = Instantiate (gold) as GameObject;
        gold.transform.SetParent(boardUI.transform, false);

        helium = Instantiate (helium) as GameObject;
        helium.transform.SetParent(boardUI.transform, false);

        hydrogen = Instantiate (hydrogen) as GameObject;
        hydrogen.transform.SetParent(boardUI.transform, false);

        krypton = Instantiate (krypton) as GameObject;
        krypton.transform.SetParent(boardUI.transform, false);

        neon = Instantiate (neon) as GameObject;
        neon.transform.SetParent(boardUI.transform, false);

        nickel = Instantiate (nickel) as GameObject;
        nickel.transform.SetParent(boardUI.transform, false);

        nitrogen = Instantiate (nitrogen) as GameObject;
        nitrogen.transform.SetParent(boardUI.transform, false);

        oxygen = Instantiate (oxygen) as GameObject;
        oxygen.transform.SetParent(boardUI.transform, false);

        potassium = Instantiate (potassium) as GameObject;
        potassium.transform.SetParent(boardUI.transform, false);

        silver = Instantiate (silver) as GameObject;
        silver.transform.SetParent(boardUI.transform, false);

        sodium = Instantiate (sodium) as GameObject;
        sodium.transform.SetParent(boardUI.transform, false);

        xenon = Instantiate (xenon) as GameObject;
        xenon.transform.SetParent(boardUI.transform, false);

    }
}
