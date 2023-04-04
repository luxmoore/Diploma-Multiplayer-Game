using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>This scipt hosts the functions that pathfind on and create the grid. Not functional by itself, has to be procced elsewhere.</summary>

public class LUX_Grid : MonoBehaviour
{
    #region Variables

    // Private
    private Vector3 x0y0Location = Vector3.zero;
    private int debugTicker = 0;

    private GameController gameController;

    // Public
    [Tooltip("This gameobject is what all of the gridbits will be parented to. Cannot be a prefab from inspector. Generally just make this a new empty.")]
    public GameObject gridHolderEmpty;
    [Tooltip("A gridbit can be anything, only thing that matters is that the prefab supplied has the 'LUX_GridBit' script in it.")]
    public GameObject gridBitPrefab;
    [Tooltip("The list of gameobjects that make a path from the start to the end.")]
    public List<GameObject> pathList = new List<GameObject>();
    [Tooltip("This holds all of the gridbits.")]
    public GameObject[,] localGridArray;

    [HideInInspector]
    public Vector2 pathfindingObjective = new Vector2();
    public GameObject currentPathFollowingAgent;
    public int gridWidth;
    public int gridHeight;

    [Header("Debug")]
    public Vector2 debugStartXY;
    public Vector2 debugEndXY;
    public bool hitIt;
    public bool debugOut;

    [Space(5)]
    public Material debugMatBAD;
    public Material debugMatGUY;
    public Material debugMatPATH;

    #endregion

    #region Grid Generation

    private void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
    }

    public GameObject[,] GridGen(int width, int height, int holesAmount)
    {
        GameObject[,] funcReturnVal = new GameObject[width, height];

        gridWidth = width;
        gridHeight = height;

        // this generates up a column, then moves to the side and generates another column.

        for(int tickerA = 0; tickerA < width; tickerA++) // tickerA is width, or, x
        {
            for(int tickerB = 0; tickerB < height; tickerB++) // tickerB is height, or, y
            {
                GameObject obj = Instantiate(gridBitPrefab, new Vector3(x0y0Location.x + tickerA, x0y0Location.y, x0y0Location.z + tickerB), Quaternion.identity);
                obj.transform.SetParent(gridHolderEmpty.transform);
                obj.GetComponent<LUX_GridBit>().gridPos.x = tickerA;
                obj.GetComponent<LUX_GridBit>().gridPos.y = tickerB;
                funcReturnVal[tickerA, tickerB] = obj;
                if (debugOut == true) { Debug.Log("Generated grid bit, position " + tickerA + ", " + tickerB); }
                obj.name = "GridBit " + tickerA + ", " + tickerB;
                obj.tag = "GridBit";
            }
        }

        localGridArray = funcReturnVal;

        if (debugOut == true) { Debug.Log("Poking out " + holesAmount + " gridbits."); }
        PokeRandomHole(holesAmount);


        if (debugOut == true) { Debug.Log("Placing players"); }
        PlacePlayersOnGrid();

        return (funcReturnVal);
    }

    private void PokeRandomHole(int amount)
    {
        Vector2 pokeoutSpot = Vector2.zero;
        GameObject obj;

        for(int ticker = 0; ticker < amount; ticker++)
        {
            pokeoutSpot.x = Random.Range(0, gridWidth);
            pokeoutSpot.y = Random.Range(0, gridHeight);

            if (debugOut == true) { Debug.Log("Poking out spot at " + pokeoutSpot.x + ", " + pokeoutSpot.y); }

            obj = localGridArray[(int)pokeoutSpot.x, (int)pokeoutSpot.y];
            obj.GetComponent<LUX_GridBit>().isWalkable = false;
            obj.GetComponent<MeshRenderer>().enabled = false;
            obj.GetComponent<LUX_GridBit>().canvasObj.SetActive(false);
            obj.tag = "Dead GridBit";
        }
    }

    private void PlacePlayersOnGrid()
    {
        // get the amount of players (counted from array)
        // get two random ints and check against grid if possible

        int randX;
        int randY;
        int playerAmount = gameController.alivePlayers.Count;
        int tries;

        for(int ticker = 0; ticker < playerAmount; ticker++)
        {
            bool cont = true;
            tries = 0;

            while(cont == true)
            {
                tries++;
                randX = Random.Range(0, gridWidth);
                randY = Random.Range(0, gridHeight);
                LUX_GridBit selectedGridBit = localGridArray[randX, randY].GetComponent<LUX_GridBit>();
                if (selectedGridBit.isWalkable == true && selectedGridBit.playerOnThis == false)
                {
                    // place already spawned player on gridbit
                    // set playerOnThis to be true
                    // allow the for-loop to progress by one

                    gameController.alivePlayers[ticker].GetComponentInChildren<PlayerStats>().gridPos = new Vector2(randX, randY);
                    gameController.alivePlayers[ticker].transform.position = new Vector3(randX, 1, randY);

                    if (debugOut == true) { Debug.Log("Placed player number " + ticker + " at position " + randX + ", " + randY + " after " + tries + " attempt(s)"); }

                    selectedGridBit.playerOnThis = true;

                    cont = false;
                }
                if(tries == 50)
                {
                    Debug.Log("NO SPAWN FOR YOU >:)");
                    cont = false;
                }
            }
        }

    }

    #endregion

    #region Pathfinding Functions

    public void SetAllVisitedNegative(Vector2 currentXY)
    {
        foreach(GameObject obj in localGridArray)
        {
            if(obj.GetComponent<LUX_GridBit>().isWalkable == false)
            {
                obj.GetComponent<LUX_GridBit>().visited = -2;
            }
            else
            {
                obj.GetComponent<LUX_GridBit>().visited = -1;
            }
        }
        localGridArray[(int)currentXY.x, (int)currentXY.y].GetComponent<LUX_GridBit>().visited = 0;
        localGridArray[(int)currentXY.x, (int)currentXY.y].GetComponent<MeshRenderer>().material = debugMatGUY;
    }

    private bool TestADirectionFrom(Vector2 fromXY, int step, int direction, int debug)
    {
        // This function tests the direction asked of it, by the int direction. The direction chosen is 1 for up, 2 for right and so on clockwise.
        switch (direction)
        {
            #region 1 - UP
            case 1:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if 
                (
                    ((int)fromXY.y + 1) < gridHeight 
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().visited == step
                ) { if (debugOut == true) { Debug.Log("Tested up from " + fromXY + " Positive result. Tested for " + debug); } return true;}
                else { if (debugOut == true) { Debug.Log("Tested up from " + fromXY + " Negative result. Tested for " + debug); } return false;}
            #endregion

            #region 2 - RIGHT
            case 2:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if 
                (
                    ((int)fromXY.x + 1) < gridWidth
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().visited == step
                ) { if (debugOut == true) { Debug.Log("Tested right from " + fromXY + " Positive result. Tested for " + debug); } return true;}
                else { if (debugOut == true) { Debug.Log("Tested rightfrom  " + fromXY + " Negative result. Tested for " + debug); } return false;}
            #endregion

            #region 3 - DOWN
            case 3:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if
                (
                    ((int)fromXY.y - 1) > -1
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().visited == step
                ) { if (debugOut == true) { Debug.Log("Tested down from " + fromXY + " Positive result. Tested for " + debug); } return true;}
                else { if (debugOut == true) { Debug.Log("Tested down from " + fromXY + " Negative result. Tested for " + debug); } return false;}
            #endregion

            #region 4 - LEFT
            case 4:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if 
                (
                    ((int)fromXY.x - 1) > -1 
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().visited == step
                ) 
                { if (debugOut == true) { Debug.Log("Tested left from " + fromXY + ". Positive result. Tested for " + debug); } return true;}
                else { if (debugOut == true) { Debug.Log("Tested left from " + fromXY + " Negative result. Tested for " + debug); } return false;}
            #endregion

            #region 5 - UP + LEFT
            case 5:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if
                (
                    //left
                    ((int)fromXY.x - 1) > -1
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    //up
                    ((int)fromXY.y + 1) < gridHeight
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    // up + left
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y + 1)]
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().visited == step
                )
                { if (debugOut == true) { Debug.Log("Tested diagonal up-left from " + fromXY + ". Positive result. Tested for " + debug); } return true; }
                else { if (debugOut == true) { Debug.Log("Tested diagonal up-left from " + fromXY + " Negative result. Tested for " + debug); } return false; }
            #endregion

            #region 6 - UP + RIGHT
            case 6:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if
                (
                    //right 
                    ((int)fromXY.x + 1) < gridWidth
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    //up
                    ((int)fromXY.y + 1) < gridHeight
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    //up + right
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y + 1)]
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().visited == step
                ) { if (debugOut == true) { Debug.Log("Tested up-right from " + fromXY + " Positive result. Tested for " + debug); } return true; }
                else { if (debugOut == true) { Debug.Log("Tested up-rightfrom  " + fromXY + " Negative result. Tested for " + debug); } return false; }
            #endregion

            #region 7 - DOWN + LEFT
            case 7:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if
                (
                    //left
                    ((int)fromXY.x - 1) > -1
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    //down
                    ((int)fromXY.y - 1) > -1
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    //down + left
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y - 1)]
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x - 1, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().visited == step
                )
                { if (debugOut == true) { Debug.Log("Tested diagonal up-left from " + fromXY + ". Positive result. Tested for " + debug); } return true; }
                else { if (debugOut == true) { Debug.Log("Tested diagonal up-left from " + fromXY + " Negative result. Tested for " + debug); } return false; }
            #endregion

            #region 8 - DOWN + RIGHT
            case 8:
                if (debugOut == true) { debugTicker++; Debug.Log("Check number " + debugTicker + " is:"); }
                if
                (
                    //right 
                    ((int)fromXY.x + 1) < gridWidth
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    //down
                    ((int)fromXY.y - 1) > -1
                    &&
                    localGridArray[(int)fromXY.x, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().isWalkable == true

                    &&

                    //down + right
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y - 1)]
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().isWalkable == true
                    &&
                    localGridArray[(int)fromXY.x + 1, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().visited == step
                ) { if (debugOut == true) { Debug.Log("Tested up-right from " + fromXY + " Positive result. Tested for " + debug); } return true; }
                else { if (debugOut == true) { Debug.Log("Tested up-rightfrom  " + fromXY + " Negative result. Tested for " + debug); } return false; }
            #endregion

            case >= 9:
                Debug.Log("You dumbass, you went too high. One to eight next time silly.");
                return false;
        }

        Debug.Log("WARNING. TestFourDirectionsFrom() has fucked up.");
        return false; // failsafe
    }

    private void TestAllDirections(Vector2 fromXY, int step, bool overWrite, int testingFor)
    {
        if (TestADirectionFrom(fromXY, testingFor, 1, step)) // up
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x, fromXY.y + 1), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x, (int)fromXY.y + 1];
                return;
            }
        }

        if (TestADirectionFrom(fromXY, testingFor, 2, step)) // right
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x + 1, fromXY.y), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x + 1, (int)fromXY.y];
                return;
            }
        }     

        if (TestADirectionFrom(fromXY, testingFor, 3, step)) // down
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x, fromXY.y - 1), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x, (int)fromXY.y - 1];
                return;
            }
        }

        if (TestADirectionFrom(fromXY, testingFor, 4, step)) // left
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x - 1, fromXY.y), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x - 1, (int)fromXY.y];
                return;
            }
        }

        if (TestADirectionFrom(fromXY, testingFor, 5, step)) // up + left
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x - 1, fromXY.y + 1), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x - 1, (int)fromXY.y + 1];
                return;
            }
        }

        if (TestADirectionFrom(fromXY, testingFor, 6, step)) // up + right
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x + 1, fromXY.y + 1), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x + 1, (int)fromXY.y + 1];
                return;
            }
        }

        if (TestADirectionFrom(fromXY, testingFor, 7, step)) // down + left
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x - 1, fromXY.y - 1), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x - 1, (int)fromXY.y - 1];
                return;
            }
        } 
        if (TestADirectionFrom(fromXY, testingFor, 8, step)) // down + right
        {
            if (overWrite)
            {
                SetVisitedVarOnGridBit(new Vector2(fromXY.x + 1, fromXY.y - 1), step);
            } else
            {
                nextPathBit = localGridArray[(int)fromXY.x + 1, (int)fromXY.y - 1];
                return;
            }
        } 
    }

    private void SetVisitedVarOnGridBit(Vector2 particularGridbitXY, int step)
    {
        if (localGridArray[(int)particularGridbitXY.x, (int)particularGridbitXY.y])
        {
            localGridArray[(int)particularGridbitXY.x, (int)particularGridbitXY.y].GetComponent<LUX_GridBit>().visited = step;
            localGridArray[(int)particularGridbitXY.x, (int)particularGridbitXY.y].GetComponent<MeshRenderer>().material = debugMatBAD;
        }
    }

    public void SetDistance(Vector2 drawFromXY)
    {
        int[] testArray = new int[gridWidth * gridHeight];
        for (int step = 1; step < gridWidth * gridHeight; step++)
        {
            foreach(GameObject obj in localGridArray)
            {
                if(obj.GetComponent<LUX_GridBit>().visited == step - 1)
                {
                    TestAllDirections(obj.GetComponent<LUX_GridBit>().gridPos, step, true, -1);
                }
            }
        }
    }

    private GameObject nextPathBit;
    public void CreatePath(Vector2 fromXY, Vector2 toXY) 
    {
        #region Explanation

        // This code walks backwards from the gridbit specified by the toXY local variable
        // It walks backwards by first grabbing the step count of the toXY, then moving to a neighbouring gridbit with a step count of one lower, recording the path
        // This repeats until the stepcount is zero, at which point we have a path

        #endregion

        // Issues:
        // None. awesome code, great job!

        pathList.Clear();
        nextPathBit = null;

        // Where the code should create a path to
        int toX = (int) toXY.x;
        int toY = (int) toXY.y;

        // Where the path should end back up at
        int fromX = (int) fromXY.x;
        int fromY = (int) fromXY.y;

        // Because the path is initially reversed, we do this backwards, then use the .Reverse() function

        int maxSteps = localGridArray[fromX, fromY].GetComponent<LUX_GridBit>().visited;
        // maxSteps is set to how many steps it minimally takes to reach a specific gridbit

        if (debugOut == true) { Debug.Log("Attempting to move from " + fromXY + " to " + toXY + ", with " + maxSteps + " maximum movements"); }

        nextPathBit = localGridArray[fromX, fromY];
        pathList.Add(nextPathBit);
        if (debugOut == true) { Debug.Log("Added first gridbit successfully from position: " + nextPathBit.GetComponent<LUX_GridBit>().gridPos); }

        for(int step = maxSteps -1; step != 0; step--)
        {
            if (debugOut == true) { Debug.Log("Step attempt number " + (maxSteps - step) + " is from " + nextPathBit.GetComponent<LUX_GridBit>().gridPos); }
            TestAllDirections(nextPathBit.GetComponent<LUX_GridBit>().gridPos, maxSteps - 1, false, step);

            if(pathList.Contains(nextPathBit))
            {
                if (debugOut == true) { Debug.Log("The gridbit at " + nextPathBit.GetComponent<LUX_GridBit>().gridPos + " is already inside of pathList"); }
            }
            else
            {
                pathList.Add(nextPathBit);
                if (debugOut == true) { Debug.Log("Added gridbit number " + step + " at " + nextPathBit.GetComponent<LUX_GridBit>().gridPos); }
            }
        }
        if (debugOut == true) { Debug.Log("Added last gridbit"); }
        pathList.Add(localGridArray[toX, toY]);
        pathList.Reverse();
    }

    #endregion

    #region Input Output

    public void ReceiveSelection(Vector2 selection, int playerNum)
    {
        Vector2 playerPos = Vector2.zero;

        playerPos = gameController.alivePlayers[playerNum].GetComponentInChildren<PlayerStats>().gridPos;

        Debug.Log("Selection recieved as being " + selection + " from player number " + playerNum + ", who is located at position " + playerPos);

        SetAllVisitedNegative(playerPos);
        SetDistance(playerPos);
        CreatePath(selection, playerPos);

        gameController.alivePlayers[playerNum].GetComponent<LUX_PathFollower>().StartCoroutine("STATE_MOVE");
    }

    public void SetUpGo(Vector2 playerPos)
    {
        SetAllVisitedNegative(playerPos);
        SetDistance(playerPos);
    }

    private void Update()
    {
        if (hitIt == true)
        {
            hitIt = false;
            SetAllVisitedNegative(debugStartXY);
            SetDistance(debugStartXY);
            CreatePath(debugEndXY, debugStartXY);
            gameController.alivePlayers[0].GetComponent<LUX_PathFollower>().StartCoroutine("STATE_MOVE");
        }
    }
    #endregion

    // LUX
}