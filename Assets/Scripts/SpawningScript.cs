using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SpawningScript : NetworkBehaviour {

    bool[,] checkArray;
    int last;
    private GameObject spawnList;
    ArrayList spawnPoints;
    BoardScript p1Board;
    public GameObject Element;

	// Use this for initialization
	void Start () {
        checkArray = new bool[4, 4];
        last = -1;
        spawnPoints = new ArrayList();

        
    }


    [Command]
	void CmdspawnElement(int val, Vector3 trans)
    {
        GameObject instance;
        Element.GetComponent<ElementScript>().elementID = val;
        instance = Instantiate(Element, new Vector3(trans.x, trans.y, trans.z), new Quaternion(0, 0, 0, 0)) as GameObject;
        NetworkServer.Spawn(instance);


    }
    // Update is called once per frame
    void Update () {

        if (p1Board == null && GameObject.Find("Player 1")){
            p1Board = GameObject.Find("Player 1").GetComponent<BoardScript>();
            if (GameObject.Find("ELEMENT MARKERS"))
            {
                spawnList = GameObject.Find("ELEMENT MARKERS");
                for (int i = 0; i < spawnList.transform.childCount; i++)
                {
                    spawnPoints.Add(spawnList.transform.GetChild(i));
                }
            }
            for (int i = 0; i < spawnPoints.Count;)
            {
                int x = Random.Range(0, 16);
                if (checkSetup(x))
                {
                    int val = p1Board.getValueAtPoint(x / 4, x % 4);
                    CmdspawnElement(val, (spawnPoints[i] as Transform).position);
                    i++;
                }
            }
        }
           
	}

    bool checkSetup(int cur)
    {
        if (checkArray[cur / 4, cur % 4])
        {
            return false;
        }
        if (last == -1)
        {
            checkArray[cur / 4, cur % 4] = true;
            last = cur;
            return true;

        }

        int x = last /4;
        int y = last % 4;
        int x1 = cur / 4;
        int y1 = cur % 4;
        if(!((x == x1 +1) || (x== x1-1)) || !((y == y1+1) || (y == y1 - 1))){
            checkArray[cur / 4, cur % 4] = true;
            last = cur;
            return true;
        }
        return false;
    }
}
