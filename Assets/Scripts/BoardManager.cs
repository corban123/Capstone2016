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

	void Start () {
		// Generate the boards
		generator = GetComponent<BoardGenerator> ();
		List<int[,]> boards = generator.CreateBoards();
	}
}
