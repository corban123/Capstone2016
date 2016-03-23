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

	// Use this for initialization
	void Start () {
        checkArray = new bool[4, 4];
        last = -1;
        spawnPoints = new ArrayList();

        
    }


    [Command]
	void CmdspawnElement(int val, Vector3 trans, string name)
    {
        Debug.Log("NAME: " + name + " TRANS: " + trans);
        GameObject instance;
        Element.GetComponent<ElementScript>().elementID = val;

        GameObject marker = GetObject (val);

		instance = Instantiate(Element, new Vector3(trans.x, trans.y + 3, trans.z), new Quaternion(0, 0, 0, 0)) as GameObject;
        marker = Instantiate (marker, new Vector3 (trans.x, trans.y + 10, trans.z), new Quaternion (0, 0, 0, 0)) as GameObject;
        marker.transform.parent = instance.transform;
        NetworkServer.Spawn(instance);
        NetworkServer.Spawn (marker);
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
                    spawnPoints.Add(spawnList.transform.GetChild(i).transform);
                }
            }
            for (int i = 0; i < spawnPoints.Count;)
            {
                int x = Random.Range(0, 16);
                if (checkSetup(x, i))
                {
                    int val = p1Board.getValueAtPoint(x / 4, x % 4);
                    CmdspawnElement(val, (spawnPoints[i] as Transform).position, (spawnPoints[i] as Transform).gameObject.name);
                    i++;
                }
            }
        }
           
	}

    bool checkSetup(int cur, int i)
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
        if(i == 15)
        {
            return true;
        }
        return false;
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
}
