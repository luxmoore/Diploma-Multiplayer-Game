using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GridBehaviour : MetaStats
{
    public bool findDistance = false;
    private int gridWidth;
    private int gridHeight;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottom = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX, startY;
    public int endX, endY;
    public List<GameObject> path = new List<GameObject>();

    private void Awake()
    {
        //temp
        gridWidth = 10;
        gridHeight = 10;
        //endtemp

        gridArray = new GameObject[gridHeight, gridWidth];

        //Component gridStats = gameObject.GetComponent<GridStats>();

        //gridWidth = gridStats.width;
        //gridHeight = gridStats.height;
    }

    private void GridGeneration(int localWidth, int localHeight)
    {
        // With for loops: first parameter procs once at start, second defines the condition under which the code will run, third procs every time the loop finish
        // The following code essentially:
        // Creates a local variable called 'tickerA'
        // Runs through the loop, so long as the condition is met: ie, when the amount of the ticker goes past the amount of columns, it will no longer increase.
        // Every time that the for loop is run through, the ticker increases by one.
        //
        // Because the grid is not being used at this point, this is only to create gameobjects at each position in the grid
        // You can think of this code as functioning alike one of them old TV screens turned on its side:
        // It creates a series of grid spots in a line upwards, until it reaches the height designated.
        // At this point, it moves across by one and repeats the above line. This is repeated until it reaches the width designated.
        // The function is then finished.


        for (int tickerA = 0; tickerA < gridHeight; tickerA = tickerA + 1)
        {
            for (int tickerB = 0; tickerB < gridWidth; tickerB = tickerB + 1)
            {
                GameObject surrogateObj = Instantiate(gridPrefab, new Vector3(leftBottom.x + scale * tickerA, leftBottom.y, leftBottom.z + scale * tickerB), Quaternion.identity);
                surrogateObj.transform.SetParent(gameObject.transform);
                surrogateObj.AddComponent<GridStats>();
                //surrogateObj.GetComponent<GridStats>().x = tickerB;
                //surrogateObj.GetComponent<GridStats>().y = tickerA;
                Debug.Log("Generated grid position " + tickerA + "," + tickerB);
                gridArray[tickerA, tickerB] = surrogateObj;
            }
        }
    }

    private void InitialSetUp()
    {
        foreach(GameObject surrogateObj in gridArray)
        {
            surrogateObj.GetComponent<GridStats>().visited = -1;
        }
        gridArray[startX,startY].GetComponent<GridStats>().visited = 0;
    }

    bool TestDirection(int localX, int localY, int step, int direction)
    {
        // int direction tells which case to use. 1 is up, 2 is right, 3 is down, 4 is left
        switch (direction)
        {
            case 1:
                if((localY + 1) < gridHeight && gridArray[localX, localY + 1] && gridArray[localX, localY + 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if ((localX + 1) < gridHeight && gridArray[localX + 1, localY] && gridArray[localX + 1, localY].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if ((localY - 1) < gridHeight && gridArray[localX, localY - 1] && gridArray[localX, localY - 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                if ((localX - 1) < gridHeight && gridArray[localX - 1, localY] && gridArray[localX - 1, localY].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    private void SetVisited(int localX, int localY, int step)
    {
        if (gridArray[localX, localY])
        {
            gridArray[localX, localY].GetComponent<GridStats>().visited = step;
        }
    }

    private void TestFourDirections(int localX, int localY, int step)
    {
        if(TestDirection(localX, localY, -1, 1))
        {
            SetVisited(localX, localY + 1, step);
        }
        if(TestDirection(localX, localY, -1, 2))
        {
            SetVisited(localX + 1, localY, step);
        }
        if(TestDirection(localX, localY, -1, 3))
        {
            SetVisited(localX, localY - 1, step);
        }
        if(TestDirection(localX, localY, -1, 4))
        {
            SetVisited(localX - 1, localY, step);
        }

    }

    private void SetDistance()
    {
        InitialSetUp();
        int localX = startX;
        int localY = startY;

        int[] testArray = new int[gridHeight * gridWidth];
        for(int step = 1; step < gridWidth; step++)
        {
            foreach(GameObject surrogateObj in gridArray)
            {
                if(surrogateObj && surrogateObj.GetComponent<GridStats>().visited == step - 1)
                {
                    TestFourDirections((int)surrogateObj.GetComponent<GridStats>().gridPos.x, (int)surrogateObj.GetComponent<GridStats>().gridPos.y, step);
                }
            }
        }
    }

    private void SetPath()
    {
        int step;
        int localX = endX;
        int localY = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if (gridArray[endX,endY] && gridArray[endX, endY].GetComponent<GridStats>().visited > 0)
        {
            path.Add(gridArray[localX, localY]);
            step = gridArray[localX, localY].GetComponent<GridStats>().visited - 1;
        }
        else
        {
            Debug.Log("Cannot reach desired location");
            return;
        }
        for(int i = step; step > -1; step--)
        {
            if (TestDirection(localX, localY, -1, 1))
            {
                SetVisited(localX, localY + 1, step);
            }
            if (TestDirection(localX, localY, -1, 2))
            {
                SetVisited(localX + 1, localY, step);
            }
            if (TestDirection(localX, localY, -1, 3))
            {
                SetVisited(localX, localY - 1, step);
            }
            if (TestDirection(localX, localY, -1, 4))
            {
                SetVisited(localX - 1, localY, step);
            }
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            localX = (int)tempObj.GetComponent<GridStats>().gridPos.x;
            localY = (int)tempObj.GetComponent<GridStats>().gridPos.y;
            tempList.Clear();
        }
    }

    private GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * gridHeight * gridWidth;
        int indexNum = 0;
        for(int tickerC = 0; tickerC < list.Count; tickerC++)
        {
            if(Vector3.Distance(targetLocation.position, list[tickerC].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[tickerC].transform.position);
                indexNum = tickerC;
            }
        }
        return list[indexNum];
    }

    private void Update()
    {
        if(findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }

    //temp
    private void Start()
    {
        GridGeneration(gridWidth, gridHeight);

    }
}