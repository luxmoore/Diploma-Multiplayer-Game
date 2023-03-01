using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

// Things I do not yet understand:
// - The relation between the test direction function and actually testing directions 

// Things Explained in these comments:
// - The generation of the grid and the way that the particular gridbit is stored


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
                surrogateObj.GetComponent<GridStats>().gridPos.x = tickerB; // neither of these are used in the function primarily
                surrogateObj.GetComponent<GridStats>().gridPos.y = tickerA; // neither of these are used in the function primarily
                Debug.Log("Generated grid position " + tickerA + "," + tickerB);
                gridArray[tickerA, tickerB] = surrogateObj;
            }
        }
    }

    private void InitialSetUp() // used at the start of assigning gridbit steps
    {
        foreach(GameObject surrogateObj in gridArray) // this goes over every gridbit in the array, setting it to be 'unexplored'.
        {
            surrogateObj.GetComponent<GridStats>().visited = -1;
        }
        gridArray[startX,startY].GetComponent<GridStats>().visited = 0; 
        
        // this last line sets the starting position of the pathfinding to be zero, the lowest possible step amount.
        // The reason we set all of the others to be '-1' is so that when counting upwards, we never reach the 'unexplored' tag (which has to be done as a number)
    }

    bool TestDirection(int localX, int localY, int step, int direction)
    {
        // the int variable 'direction' tells which case to use. 1 is up, 2 is right, 3 is down, 4 is left
        // By using the x and y of the grid, we are able to calculate what is directly to the left, right, up and down of a gridbit.
        // This function is used in the 'TestFourDirections' function. One being used, all of the directions are tested in different for loop go-arounds.
        // The step being tested is always specified as '-1'. Couldn't have written that for clarity? Had to have 700 local vars called 'step' specifically? cunt.
        // On returning true, ie, the tested gridbit is in fact '-1':
        //     The tile tested by the function is fed into the 'SetVisited' function and given a differing step value than the one specified in this function.
        // On returning false, ie, the tested gridbit is not '-1':
        //     The tile tested is passed over. its firstborn is spared by the angel of step.

        switch (direction)
        {
            case 1:
                if((localY + 1) < gridHeight && gridArray[localX, localY + 1] && gridArray[localX, localY + 1].GetComponent<GridStats>().visited == step)
                    // this (and all four other if statements check that:
                    // if one gridbit over is inside the possible gridspots (if it is larger than the set possible height, it cannot be returned as true)
                    //                                                      (a note, the sideways one is set to the width and the 'negatives' are tested against -1)
                    //
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if ((localX + 1) < gridWidth && gridArray[localX + 1, localY] && gridArray[localX + 1, localY].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if ((localY - 1) > -1 && gridArray[localX, localY - 1] && gridArray[localX, localY - 1].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                if ((localX - 1) > -1 && gridArray[localX - 1, localY] && gridArray[localX - 1, localY].GetComponent<GridStats>().visited == step)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false; // failsafe
    }

    private void SetVisited(int localX, int localY, int step) // please see 'TestDirection' for explanation
    {
        if (gridArray[localX, localY])
        {
            gridArray[localX, localY].GetComponent<GridStats>().visited = step;
        }
    }

    private void TestFourDirections(int localX, int localY, int step) // no elses, all are tested, then imprinted with the step specified.
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
        for(int step = 1; step < gridWidth; step++) // steps backward through steps, using the array to check each array member's step variable
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
        if(findDistance) // a bool that is turned on in inspector in this script
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