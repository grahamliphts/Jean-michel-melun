using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour {

    [SerializeField]
    float sprite_size;

    [SerializeField]
    int mapSize;

    [SerializeField]
    int UpRoadNb;

    [SerializeField]
    int RightRoadNb;

    [SerializeField]
    int pathNb;

    [SerializeField]
    int buildingSize;

    [SerializeField]
    int maxKeepGo;

    int startLine;
    int startColumn;

    int endLine;
    int endColumn;
    
    public List<Vector3> sideWalkList = new List<Vector3>();
    public List<Vector3> freeSpaceList = new List<Vector3>();

    Vector3 EntryPoint = new Vector3();
    Vector3 ExitPoint = new Vector3();

    public bool _finish = false;
    
    int[] endLineTab;
    int[] endColumnTab;
    int[] heightTab;

    [SerializeField]
    GameObject[] BuildingPrefab;
    [SerializeField]
    GameObject[] GroundPrefab;
    [SerializeField]
    GameObject[] PathPrefab;
    [SerializeField]
    GameObject[] RoadPrefab;
    [SerializeField]
    GameObject[] SideWalkPrefab;


    private int[][] _Map;

    int[][] orientedMap;
    
    // Use this for initialization
    void Start()
    {

        int baseMapType = 0;
        int buildType = 1;
        int pathType = 2;
        int roadType = 3;
        int sideWalkType = 5;

        // Generate base ground empty map
        initMap(baseMapType);

        // Create horizontal roads 
        defPointTabs(RightRoadNb);
        for (int count = 0; count < RightRoadNb; count++)
        {
            randomizeTab(heightTab, 2);

            startColumn = endLineTab[count];
            startLine = heightTab[0];

            endColumn = endColumnTab[count];
            endLine = heightTab[1];

            createRoad(roadType);
        }

        // Create Vertical roads 
        defPointTabs(UpRoadNb);
        for (int count = 0; count < UpRoadNb; count++)
        {
            randomizeTab(heightTab, 2);

            startLine = endLineTab[count];
            startColumn = heightTab[0];

            endLine = endColumnTab[count];
            endColumn = heightTab[1];

            createRoad(roadType);
        }

        //orientedRoad(roadType);

        // Create sidewalks
        createsideWalk(sideWalkType, baseMapType, roadType);

        // Create walkable paths
        for (int count = 0; count < pathNb; count++)
            createPath(pathType, baseMapType, sideWalkType);

        // Create buildings
        createBuilding(buildType, baseMapType, buildingSize);

        // Define Entry Point 
        int i = 0;
        while (_Map[i][0] != roadType)
            i++;
        EntryPoint.x = i;
        EntryPoint.y = 0;

        Debug.Log("Entry : " + EntryPoint.x);


        //Define Exit Point
        i = mapSize - 1;
        while (_Map[i][mapSize-1] != roadType)
            i--;
        ExitPoint.x = i;
        ExitPoint.y = mapSize-1;

        Debug.Log("Exit : " + ExitPoint.x);

        generateMap();
        _finish = true;
    }
	
	// Update is called once per frame
	void Update () {
		
    }

    //Randomize order of elements in a tab 
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

    //Define starts and end points for roads
    void defPointTabs(int roadNb)
    {
        endLineTab = new int[roadNb];
        endColumnTab = new int[roadNb];
        for (int count = 0; count < roadNb; count++)
        {
            endLineTab[count] = mapSize / (roadNb + 1) * (count + 1);
            endColumnTab[count] = mapSize / (roadNb + 1) * (count + 1);
        }
        randomizeTab(endLineTab, roadNb);
        randomizeTab(endColumnTab, roadNb);

        heightTab = new int[2];
        heightTab[0] = 0;
        heightTab[1] = mapSize - 1;
    }

    // Create area_type map
    void initMap(int area_type)
    {
        _Map = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            _Map[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                _Map[i][j] = area_type;
            }
        }
    }

    // Add an area_type road in the map
    void createRoad(int area_type)
    {
        int currPointI = startLine;
        int currPointJ = startColumn;
        _Map[startLine][startColumn] = area_type;

        int nextPointI = endLine;
        int nextPointJ = endColumn;
        _Map[endLine][endColumn] = area_type;

        bool keepWalk = true;

        int principalDir = 0;
        int currKeepGo = 0;

        while (keepWalk == true)
        {
            _Map[currPointI][currPointJ] = area_type;

            if (currKeepGo == maxKeepGo)
            {
                principalDir = Random.Range(0, 2);
                currKeepGo = 0;
            }
            else
                currKeepGo += 1;


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

    int compareNeiboors(int[] l1)
    {

        int orientation = 0;

        int[][] neiboors = new int[7][];

        neiboors[0] = new int[] {0,1,0,0,1,0,0,1,0}; //Vertical
        neiboors[1] = new int[] {0,0,0,1,1,1,0,0,0}; //Horizontal
        neiboors[2] = new int[] {0,0,0,0,1,0,1,1,1}; // Down
        neiboors[3] = new int[] {0,0,0,1,1,1,0,1,0}; // Top
        neiboors[4] = new int[] {0,1,0,0,1,1,0,1,0}; // Left
        neiboors[5] = new int[] {0,1,0,1,1,0,0,1,0}; // Right
        neiboors[6] = new int[] {0,1,0,1,1,1,0,1,0}; // Cross
        
        for (int i=0; i <7; i++)
        {
            bool res = false;
            res = compareLists(l1, neiboors[i]);

            if (res == true)

            if (res == true)
                orientation = i;
        }

        return orientation; 
    }


    bool compareLists(int[] l1, int[] l2)
    {
        bool res = true;

        for (int i = 0; i < l1.Length; i++)
        {
            if (l1[i] != l2[i])
            {
                if (res == true)
                    res = false;
            }
        }

     return res;
    }

    //Create an oriented road map 
    void orientedRoad(int area_type)
    {
        
        orientedMap = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            orientedMap[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                orientedMap[i][j] = 0;
            }
        }


        int[] neiboors = new int[9];
        for (int a=0; a < 9 ; a++)
            neiboors[a] = 0;

        int count_nei = 0;
        for (int line = 1; line < mapSize - 1; line++)
        {
            for (int col = 1; col < mapSize - 1; col++)
            {
                if (_Map[line][col] == area_type)
                {

                    count_nei = 0;
                    string debug = "";
                    for (int j = -1; j < 2; j++)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            if (_Map[line + i][col + j] == area_type)
                            {
                                neiboors[count_nei] = 1;
                            }else
                            {
                                neiboors[count_nei] = 0;
                            }

                            debug += neiboors[count_nei].ToString();
                            debug += " / ";
                            count_nei++;
                        }


                    }
                    Debug.Log(debug);
                    Debug.Log("compare :" + compareNeiboors(neiboors));
                }

                
                orientedMap[line][col] = compareNeiboors(neiboors);
                

            }      
        }
    }


    // Add an area_type path in the map between roadType roads
    void createPath(int area_type, int baseMapType, int roadType)
    {
        //Start Point of the path
        int currPointI = 0;
        int currPointJ = 0;
        while (_Map[currPointI][currPointJ] != baseMapType)
        {
            currPointI = Random.Range(0, mapSize);
            currPointJ = Random.Range(0, mapSize);
        }
        _Map[currPointI][currPointJ] = area_type;

        //End point of the path 
        int nextPointI = 0;
        int nextPointJ = 0;
        while (_Map[nextPointI][nextPointJ] != baseMapType)
        {
            nextPointI = Random.Range(0, mapSize);
            nextPointJ = Random.Range(0, mapSize);
        }

        //Connect points (except an roadType road is found) 
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

            if (_Map[currPointI][currPointJ] == roadType)
                keepWalk = false; 
            if ((currPointI == nextPointI) && (currPointJ == nextPointJ))
                keepWalk = false;
        }
    }

    // Add area_type sidewalks in the map on every roads
    void createsideWalk(int area_type, int baseMapType, int roadType)
    {
        // For first col
        for (int currJ = 1; currJ < mapSize - 1; currJ++)
        {
            if (_Map[0][currJ] == baseMapType)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_Map[i][currJ + j] == roadType)
                            _Map[0][currJ] = area_type;
                    }
                }
            }
        }

        for (int currI = 1; currI < mapSize - 1; currI++)
        {
            //For First line 
            if (_Map[currI][0] == baseMapType)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (_Map[currI + i][j] == roadType)
                            _Map[currI][0] = area_type;
                    }
                }
            }

            for (int currJ = 1; currJ < mapSize - 1; currJ++)
            {
                if (_Map[currI][currJ] == baseMapType)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (_Map[currI + i][currJ + j] == roadType)
                                _Map[currI][currJ] = area_type;
                        }
                    }
                }
            }

            //For last line 
            if (_Map[currI][mapSize-1] == baseMapType)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 1; j++)
                    {
                        if (_Map[currI + i][mapSize - 1 + j] == roadType)
                            _Map[currI][mapSize - 1] = area_type;
                    }
                }
            }
        }

        // For first col
        for (int currJ = 1; currJ < mapSize - 1; currJ++)
        {
            if (_Map[mapSize - 1][currJ] == baseMapType)
            {
                for (int i = -1; i < 1; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (_Map[mapSize - 1 + i][currJ + j] == roadType)
                            _Map[mapSize - 1][currJ] = area_type;
                    }
                }
            }
        }
    }

    //Add area_type building of buildSize in baseMapType free space availables
    void createBuilding(int area_type, int baseMapType, int buildSize)
    {
        bool isBuilding = false;
        for (int currI = 0; currI < mapSize - buildSize; currI+= buildSize)
        {
            for (int currJ = 0; currJ < mapSize - buildSize; currJ += buildSize)
            {
                if (_Map[currI][currJ] == baseMapType)
                    isBuilding = true;
                else
                    isBuilding = false;

                for (int i = 0; i < buildSize; i++)
                {
                    for (int j = 0; j < buildSize; j++)
                    {
                            if (_Map[currI + i][currJ + j] != baseMapType)
                            {
                                if (isBuilding == true)
                                    isBuilding = false;
                            }
                    }
                }

                if (isBuilding == true)
                {
                    for (int i = 0; i < buildSize; i++)
                    {
                        for (int j = 0; j < buildSize; j++)
                            _Map[currI + i][currJ + j] = area_type;
                    }
                }
            }
        }

    }

    int scaledRandom(int begin, int end, bool direction)
    {
        int[] tab_rand = new int[end];

        for (int i = 0; i < end; i++)
        {
            if (direction == true)
                tab_rand[i] = i;
            else
                tab_rand[i] = end - 1 - i; 
        }

        int nb = Random.Range(begin, end);
        int count = tab_rand[nb];

        while (count > 0)
        {
            int nb2 = Random.Range(begin, end);
            if (nb2 < nb)
            {
                nb = nb2;
                count = 0;
            }
            else
                count -= 1; 
        }
        return nb; 

    }

    void generateMap()
    {
        float i = 0;
        float j = 0;
        int nb;
        int countI = 0;
        int countJ = 0; 

        foreach(int[] MapLine in _Map)
        {
            foreach(int area in MapLine)
            {
                Vector3 position  = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                switch (area)
                {
                    case 0:     // Ground
                        Instantiate(GroundPrefab[0], position, new Quaternion(),transform);
                        freeSpaceList.Add(position);
                        break;
                    case 1:     // Building 
                        nb = scaledRandom(0, BuildingPrefab.Length, true);
                        Instantiate(BuildingPrefab[nb], position, new Quaternion(), transform);
                        break;
                    case 2:     // Path thing
                        nb = scaledRandom(0, PathPrefab.Length, true);
                        Instantiate(PathPrefab[nb], position, new Quaternion(), transform);
                        break;
                    case 3:     //Road
                        nb = scaledRandom(0, RoadPrefab.Length, true);
                        Instantiate(RoadPrefab[nb], position, new Quaternion(), transform);
                        break;

                    case 5:     //SideWalk
                        nb = scaledRandom(0, SideWalkPrefab.Length, true);
                        Instantiate(SideWalkPrefab[nb], position, new Quaternion(), transform);
                        sideWalkList.Add(position);
                        break;


                }
                j += sprite_size;

                countI++; 
            }
            countI = 0;
            j = 0;
            i+= sprite_size;

            countJ++; 
        }
    }
}
