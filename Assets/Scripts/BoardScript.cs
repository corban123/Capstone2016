﻿using UnityEngine;
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

    public Sprite barium_grey;
    public Sprite calcium_grey;
    public Sprite carbon_grey;
    public Sprite copper_grey;
    public Sprite gold_grey;
    public Sprite helium_grey;
    public Sprite hydrogen_grey;
    public Sprite krypton_grey;
    public Sprite neon_grey;
    public Sprite nickel_grey;
    public Sprite nitrogen_grey;
    public Sprite oxygen_grey;
    public Sprite potassium_grey;
    public Sprite silver_grey;
    public Sprite sodium_grey;
    public Sprite xenon_grey;

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
        GreyOutOnUI(coordinates[0], coordinates[1], element);

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

    private void GreyOutOnUI(int x, int y, int elem) {
        Image[] i = boardUI.GetComponentsInChildren<Image>();
        int idx = x * 3 + y;
        print (idx);
        i[idx].sprite = GetSprite(elem);
    }

    private void CreateBingoBoardUI () {
        int elem;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                elem = board [i, j];
                GameObject obj = GetObject (elem);
                obj = Instantiate (obj) as GameObject;
                obj.transform.SetParent (boardUI.transform, false);
            }
        }
    }

    private GameObject GetObject(int element) {
        switch (element) {
            case 0:
                return sodium;
            case 1:
                return potassium;
            case 2:
                return calcium;
            case 3:
                return barium;
            case 4:
                return copper;
            case 5:
                return nickel;
            case 6:
                return silver;
            case 7:
                return gold;
            case 8:
                return carbon;
            case 9:
                return nitrogen;
            case 10:
                return oxygen;
            case 11:
                return hydrogen;
            case 12:
                return helium;
            case 13:
                return neon;
            case 14:
                return krypton;
            case 15:
                return xenon;
            default:
                return null;
        }
    }

    private Sprite GetSprite(int element) {
        switch (element) {
            case 0:
                return sodium_grey;
            case 1:
                return potassium_grey;
            case 2:
                return calcium_grey;
            case 3:
                return barium_grey;
            case 4:
                return copper_grey;
            case 5:
                return nickel_grey;
            case 6:
                return silver_grey;
            case 7:
                return gold_grey;
            case 8:
                return carbon_grey;
            case 9:
                return nitrogen_grey;
            case 10:
                return oxygen_grey;
            case 11:
                return hydrogen_grey;
            case 12:
                return helium_grey;
            case 13:
                return neon_grey;
            case 14:
                return krypton_grey;
            case 15:
                return xenon_grey;
            default:
                return null;
            }
    }
}
