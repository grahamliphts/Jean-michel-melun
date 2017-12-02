using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour {

    [SerializeField]
    int mapSize;

    [SerializeField]
    GameObject[] BuildingPrefab;
    [SerializeField]
    GameObject[] GroundPrefab;
    [SerializeField]
    GameObject[] WalkablePrefab;


    private int[][] _Map;


	// Use this for initialization
	void Start () {
        _Map = new int[mapSize][];
        for(int i = 0; i < mapSize; i ++)
        {
            _Map[i] = new int[mapSize];
            for(int j = 0; j < mapSize; j++)
            {
                _Map[i][j] = 0;
            }
        }
        calculateMap();
        generateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void calculateMap()
    {
        for (int i = 0; i < mapSize; i++)
        {
            _Map[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                _Map[i][j] = Random.Range(0,3);
            }
        }
    }
    void generateMap()
    {
        int i = 0;
        int j = 0;
        foreach(int[] MapLine in _Map)
        {
            foreach(int area in MapLine)
            {
                Debug.Log(i + " "+ j);
                Vector3 position  = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                switch (area)
                {
                    case 0:     // Ground
                        Instantiate(GroundPrefab[0], position, new Quaternion(),transform);
                        break;;
                    case 1:     // Building
                        Instantiate(BuildingPrefab[0], position, new Quaternion(), transform);
                        break;
                    case 2:     // Walkable thing
                        Instantiate(WalkablePrefab[0], position, new Quaternion(), transform);
                        break;

                }
                j++;
            }
            j = 0;
            i++;
        }

    }
}
