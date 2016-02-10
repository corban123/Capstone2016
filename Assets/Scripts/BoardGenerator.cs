﻿using UnityEngine;
using System.Collections;
using System.Linq;

/*
 * Generates two bingo boards that are of sufficient difference.
 */
public class BoardGenerator : MonoBehaviour {
	private int THRESHOLD = 5;
	private int TIME = 10;
	private int MAX = 10;
	private int BOARD_SIZE = 16;
	private bool IS_DEBUG = true;

	// Use this for initialization
	void Start () {
		CreateBoards ();
	}

	// Create boards until they are sufficiently different.
	// If no solution is converged on by the time limit,
	// default to prechosen boards.
	void CreateBoards()
	{
		int same_factor = MAX;
		int timeout = TIME;
		int[,] board1;
		int[,] board2;

		while (same_factor >= THRESHOLD && timeout > 0) 
		{
			board1 = GenerateBoard ();
			board2 = GenerateBoard ();
			same_factor = NumSame (board1, board2);
			timeout--;
			if (IS_DEBUG) {
				PrintBoard (board1);
				PrintBoard (board2);
				print (NumSame (board1, board2));
			}
		}

		//TODO(@paige): make defaults
	}

	// Print out a board
	void PrintBoard(int[,] board)
	{
		string result = "";
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				result += (board [i, j] + " ");
			}
			result += "; ";
		}
		print(result);
	}

	// Generate a single bingo board by randomly arranging elements
	private int[,] GenerateBoard ()
	{
		int[] board = Enumerable.Range (0, BOARD_SIZE).ToArray();
		int max_index = BOARD_SIZE - 1;

		for (int i = 0; i < BOARD_SIZE - 1; i++) 
		{
			int rand_index = Random.Range (0, max_index);
			swap (board, rand_index, max_index);
			max_index--;
		}

		return to2D (board);
	}

	// Swap two elements in an array
	private static void swap(int[] array, int index1, int index2)
	{
		int temp = array [index1];
		array [index1] = array [index2];
		array [index2] = temp;
	}

	// Convert a 16 element 1d array into a 4x4 2d array
	private int[,] to2D(int[] array)
	{
		int[,] result = new int[4, 4];
		int idx = 0;
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				result [i, j] = array [idx];
				idx++;
			}
		}

		return result;
	}
		
	// Check if the bingo boards have different solutions
	private int NumSame(int[,] board1, int[,] board2) {
		int num_same = 0;

		for (int idx = 0; idx < 3; idx++) {
			// Get value from board 1
			int val = board1[idx, idx];

			// Get row and col of board 1
			int[] row1 = { board1 [idx, 0], board1 [idx, 1], board1 [idx, 2], board1 [idx, 3] };
			int[] col1 = { board1 [0, idx], board1 [1, idx], board1 [2, idx], board1 [3, idx] };

			// Find val in board 2
			int[] c = GetCoordinates(val, board2);

			// Get row and col of board 2
			int[] row2 = { board2 [c[0], 0], board2 [c[0], 1], board2 [c[0], 2], board2 [c[0], 3] };
			int[] col2 = { board2 [0, c[1]], board2 [1, c[1]], board2 [2, c[1]], board2 [3, c[1]] };

			// Compare to see if same
			if (Enumerable.SequenceEqual (row1.OrderBy(t => t), row2.OrderBy(t => t))) {
				num_same++;
			}
			if (Enumerable.SequenceEqual (row1.OrderBy(t => t), col2.OrderBy(t => t))) {
				num_same++;
			}
			if (Enumerable.SequenceEqual (col1.OrderBy(t => t), row2.OrderBy(t => t))) {
				num_same++;
			}
			if (Enumerable.SequenceEqual (col1.OrderBy(t => t), col2.OrderBy(t => t))) {
				num_same++;
			}
		}

		return num_same;
	}

	// Get coordinates of a value in the board
	private int[] GetCoordinates(int val, int[,] board) {
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
}
