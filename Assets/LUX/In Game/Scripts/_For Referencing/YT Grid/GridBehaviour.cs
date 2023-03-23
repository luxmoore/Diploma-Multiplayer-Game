using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

// An explanation of the WaveFront Pathfinding Algorithm on a square based grid.
// The way this works is by getting a point, we'll call this the starting spot.
// From this starting spot, it goes one out in all directions.
// Each time it reaches a new spot, it imprints that spot with a number.
// This number is one above the last spot it was on.
// When it reaches its' goal, it ceases to explore new spots.
// Instead, it will start again, this time from the end spot.
// Rather than searching and imprinting new numbers, it will check for the lowest value.
// This continues until it has reached the begginning spot.
// This returns as a List of gameobjects that resemble a path.

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
        // During the initial stage, the 'step' variable being tested for is '-1', however, this function is reused in walking backwards. ('step' = last step minus one)
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
        if(TestDirection(localX, localY, -1, 1)) // this only tests for unexplored bits. Indicated by -1.
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

    private void SetDistance() // this function sets out all of the visited values. The visited values can be thought of as how many steps it took to get there.
    {
        InitialSetUp(); // sets all of the gridbits' visited variable to -1
        int localX = startX;
        int localY = startY;

        int[] testArray = new int[gridHeight * gridWidth]; // returns the size of the array. Like the area of a square in geometry. Multiply the sides.
        for(int step = 1; step < gridWidth; step++) 
        {
            foreach(GameObject surrogateObj in gridArray) // this goes over every gridbit in the array.
            {
                if(surrogateObj && surrogateObj.GetComponent<GridStats>().visited == step - 1) 
                {
                    // if the particular gridbit being pointed at by the foreach loop has a step value of one less than the current step,
                    // then imprint the current step in all four directions as long as they are not -1.

                    TestFourDirections((int)surrogateObj.GetComponent<GridStats>().gridPos.x, (int)surrogateObj.GetComponent<GridStats>().gridPos.y, step);
                }
            }
        }
    }

    private void SetPath()
    {
        int step;
        int localX = endX; // the second starting point is the end
        int localY = endY; // the second starting point is the end
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