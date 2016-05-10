using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System;

/*
 * Board object for storing board, scoring, and printing.
 */
public class BoardScript : NetworkBehaviour {
	private int[,] board;
	private bool[,] scored = new bool[4, 4];
    private GameObject boardUI;
    private GameObject boardChips;
    private bool UICreated = false;

    public GameObject chipPlaceHolder;
    public Sprite chip;

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

    public Sprite barium_color;
    public Sprite calcium_color;
    public Sprite carbon_color;
    public Sprite copper_color;
    public Sprite gold_color;
    public Sprite helium_color;
    public Sprite hydrogen_color;
    public Sprite krypton_color;
    public Sprite neon_color;
    public Sprite nickel_color;
    public Sprite nitrogen_color;
    public Sprite oxygen_color;
    public Sprite potassium_color;
    public Sprite silver_color;
    public Sprite sodium_color;
    public Sprite xenon_color;

	void Start() {
	}


	// Set the board value
	public void SetBoard (int[,] board) {
        if (isLocalPlayer) {
            boardUI = GameObject.Find ("BingoBoard");
            boardChips = GameObject.Find ("BingoChips");
            this.board = board;
            StartCoroutine(CreateBoardCoroutine ());
        }
	}

    IEnumerator CreateBoardCoroutine() {
        CreateBingoBoardUI ();
        yield return new WaitForSeconds(2.0f);
    }

	// Marks an element as scored on this board
	// Returns whether or not the score causes this board to win.
	public bool score (int element) {
        if (isLocalPlayer) {
            // Find the element on the board
            int[] coordinates = GetCoordinates (element);

            // Mark the element as scored in the scored bitmap
            scored [coordinates [0], coordinates [1]] = true;

            PlaceChip (coordinates [0], coordinates [1]);

            // Change Board text
            GameObject otherPlayer;
            if (gameObject.name.Contains("1"))
                otherPlayer = GameObject.Find("Player 2");
            else 
                otherPlayer = GameObject.Find("Player 1");

            NetworkInstanceId id = otherPlayer.GetComponent<NetworkIdentity>().netId;
            bool win = isWin (coordinates [0], coordinates [1]);

            if (win) {
                gameObject.GetComponent<GUIScript> ().enableWinCanvas ();
            }

            if (isServer)
                RpcGreyOut (id, element, win);
            else
                CmdGreyOut (id, element, win);

            // Check whether the board is a winner
            return win;
        }
        return false;
	}

    [Command]
    public void CmdGreyOut(NetworkInstanceId id, int element, bool isWin) {
        GameObject player = NetworkServer.FindLocalObject (id);
        BoardScript board = player.GetComponent<BoardScript> ();
        GUIScript gui = player.GetComponent<GUIScript> ();
        try {
            board.GreyOutOnUI(element);
            if (isWin) {
                gui.enableLoseCanvas();
            } else {
                gui.enableEnemyScored();
            }
        } catch (NullReferenceException e) {
            print ("board not found: " + e);
        }
    }

    [ClientRpc]
    public void RpcGreyOut (NetworkInstanceId id, int element, bool isWin) {
        GameObject player = ClientScene.FindLocalObject (id);
        BoardScript board = player.GetComponent<BoardScript> ();
        GUIScript gui = player.GetComponent<GUIScript> ();
        try {
            board.GreyOutOnUI(element);
            if (isWin) {
                gui.enableLoseCanvas();
            } else {
                gui.enableEnemyScored();
            }
        } catch (NullReferenceException e) {
            print ("board not found: " + e);
        }

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

    private void GreyOutOnUI(int elem) {
        int[] c = GetCoordinates (elem);
        Image[] i = boardUI.GetComponentsInChildren<Image>();
        int idx = c[0] * 4 + c[1];
        i[idx].sprite = GetGreySprite(elem);
    }

    private void PlaceChip(int x, int y) {
        Image[] i = boardChips.GetComponentsInChildren<Image> ();
        int idx = x * 4 + y;
        Image chipImage = i [idx];
        chipImage.sprite = chip;

        Color c = chipImage.color;
        c.a = 255;
        chipImage.color = c;
    }

    private void CreateBingoBoardUI () {
        int elem;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                elem = board [i, j];
                GameObject obj = GetObject (elem);
                obj = Instantiate (obj) as GameObject;
                obj.transform.SetParent (boardUI.transform, false);

                GameObject tempChip = chipPlaceHolder;
                tempChip = Instantiate (tempChip) as GameObject;
                tempChip.transform.SetParent (boardChips.transform, false);
            }
        }
    }

    public GameObject GetObject(int element) {
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

    public Sprite GetGreySprite(int element) {
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

    public Sprite GetColorSprite(int element) {
        switch (element) {
        case 0:
            return sodium_color;
        case 1:
            return potassium_color;
        case 2:
            return calcium_color;
        case 3:
            return barium_color;
        case 4:
            return copper_color;
        case 5:
            return nickel_color;
        case 6:
            return silver_color;
        case 7:
            return gold_color;
        case 8:
            return carbon_color;
        case 9:
            return nitrogen_color;
        case 10:
            return oxygen_color;
        case 11:
            return hydrogen_color;
        case 12:
            return helium_color;
        case 13:
            return neon_color;
        case 14:
            return krypton_color;
        case 15:
            return xenon_color;
        default:
            return null;
        }
    }
}
