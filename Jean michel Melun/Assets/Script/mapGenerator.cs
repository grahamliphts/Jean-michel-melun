using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour {

    [SerializeField]
    int mapSize;

    [SerializeField]
    int vertPathNb;

    [SerializeField]
    int horiPathNb;

    [SerializeField]
    int alleyNb;

    [SerializeField]
    int maxKeepGo;

    int startPointI;
    int startPointJ;

    int endPointI;
    int endPointJ;

    int[] startPointTab;
    int[] endPointTab;
    int[] heightTab; 

    [SerializeField]
    GameObject[] BuildingPrefab;
    [SerializeField]
    GameObject[] GroundPrefab;
    [SerializeField]
    GameObject[] WalkablePrefab;

    private int[][] _Map;


	// Use this for initialization
	void Start ()
    {
        initMap();

        //Create Vertical roads 
        defPointTabs(vertPathNb);
        for (int count = 0; count < vertPathNb; count++)
        {
            randomizeTab(heightTab, 2);

            startPointI = startPointTab[count];
            startPointJ = heightTab[0];

            _Map[startPointI][startPointJ] = 0;

            endPointI =endPointTab[count];
            endPointJ = heightTab[1];
            _Map[endPointI][endPointJ] = 0;

            createRoad(0, true);
        }

        //Create Horizontal roads 
        defPointTabs(horiPathNb);
        for (int count = 0; count < horiPathNb; count++)
        {
            randomizeTab(heightTab, 2);

            startPointI = startPointTab[count];
            startPointJ = heightTab[0];
            _Map[startPointJ][startPointI] = 0;

            endPointI = endPointTab[count];
            endPointJ = heightTab[1];
            _Map[startPointJ][endPointI] = 0;

            createRoad(0, false);
        }

        //Create walkable alleys 
        for (int count = 0; count < alleyNb; count++)
        {
            createWalks(2);
        }

        generateMap();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void initMap()
    { 
        _Map = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            _Map[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                _Map[i][j] = 1;
            }
        }
    }

    void randomizeTab(int[] tab, int size)
    {
        int rand_num = 0;
        int temp = 0;

        for (int i = 0; i < size; i++)
        {
            rand_num = Random.Range(0, size);
            
            temp = tab[i];
            tab[i] = tab[rand_num];
            tab[rand_num] = temp;
        }
    }
    

    void defPointTabs(int roadNb)
    {
        startPointTab = new int[roadNb];
        endPointTab = new int[roadNb];
        for (int count = 0; count < roadNb; count++)
        {
            startPointTab[count] = mapSize / (roadNb + 1) * (count + 1);
            endPointTab[count] = mapSize / (roadNb + 1) * (count + 1);
        }
        randomizeTab(startPointTab, roadNb);
        randomizeTab(endPointTab, roadNb);

        heightTab = new int[2];
        heightTab[0] = 0;
        heightTab[1] = mapSize - 1;
    }


    void createPath(int area_type, bool BottomUp)
    {
        int currPointI = startPointI;
        int currPointJ = startPointJ;

        int curr_dir = 0;
        int prev_dir = 0;

        while (currPointJ < endPointJ) // Find Up-Exit
        {
            switch (curr_dir)
            {
                case 0:     // go up
                    if (currPointJ < mapSize - 2)
                        currPointJ = currPointJ + 1;
                    break;
                case 1:     // go right
                    if (currPointI < mapSize - 2)
                        currPointI = currPointI + 1;
                    else
                        currPointJ = currPointJ + 1; 
                    break;
                case 2:     // go left
                    if (currPointI > 0)
                        currPointI = currPointI - 1;
                    else
                        currPointJ = currPointJ + 1;
                    break;
            }

            //Change area type
            if (BottomUp == true)
                _Map[currPointI][currPointJ] = area_type;
            else // LeftRight
                _Map[currPointJ][currPointI] = area_type;

            //Get new direction 
            
            if (prev_dir == 0)
                curr_dir = Random.Range(1, 3); // New Direction
            else
                curr_dir = Random.Range(0, 3); // New Direction
            prev_dir = curr_dir;
            

        }

        while (currPointI < endPointI) // Find Side-Exit
        {
            if (currPointI > endPointI)
                currPointI = currPointI - 1;
            else
                currPointI = currPointI + 1;

            //Change area type
            if (BottomUp == true)
                _Map[currPointI][currPointJ] = area_type;
            else // LeftRight
                _Map[currPointJ][currPointI] = area_type;
        }
    }


    void createRoad(int area_type, bool BottomUp)
    {

        int currPointI = startPointI;
        int currPointJ = startPointJ;

        int nextPointI = endPointI;
        int nextPointJ = endPointJ;

        bool keepWalk = true;

        int principalDir = 0;
        int currKeepGo = 0;

        while (keepWalk == true)
        {
            if (BottomUp == true)
                _Map[currPointI][currPointJ] = area_type;
            else
                _Map[currPointJ][currPointI] = area_type;


            if (currKeepGo == maxKeepGo)
            {
                principalDir = Random.Range(0, 2);
                currKeepGo = 0; 
            }
            else
            {
                currKeepGo += 1; 
            }


            if (principalDir == 0) // Go Up or down
            {
                if (currPointJ > nextPointJ)
                    currPointJ = currPointJ - 1;
                else if (currPointJ < nextPointJ)
                    currPointJ = currPointJ + 1;

            }

            else // Go left or right
            {
                if (currPointI > nextPointI)
                    currPointI = currPointI - 1;
                else if (currPointI < nextPointI)
                    currPointI = currPointI + 1;
            }


            if ((currPointI == nextPointI) && (currPointJ == nextPointJ))
                keepWalk = false;
        }
    }


    void createWalks(int area_type)
    {

        int currPointI = Random.Range(0, mapSize);
        int currPointJ = Random.Range(0, mapSize);
        _Map[currPointI][currPointJ] = area_type;

        int nextPointI = Random.Range(0, mapSize);
        int nextPointJ = Random.Range(0, mapSize);

        bool keepWalk = true;
        
        

        while (keepWalk == true) 
        {
            _Map[currPointI][currPointJ] = area_type;

            int testDir = Random.Range(0, 2);

            if (testDir == 0) // Go Up or down
            {
                if (currPointJ > nextPointJ)
                    currPointJ = currPointJ - 1;
                else if(currPointJ < nextPointJ)
                    currPointJ = currPointJ + 1;
                    
            }

            else // Go left or right
            {
                if (currPointI > nextPointI)
                    currPointI = currPointI - 1;
                else if (currPointI < nextPointI)
                    currPointI = currPointI + 1;
            }

            if (_Map[currPointI][currPointJ] == 0)
                keepWalk = false; 
            if ((currPointI == nextPointI) && (currPointJ == nextPointJ))
                keepWalk = false;
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
                //Debug.Log(i + " "+ j);
                Vector3 position  = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                switch (area)
                {
                    case 0:     // Ground
                        Instantiate(GroundPrefab[0], position, new Quaternion(),transform);
                        break;
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
