using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// This script is designed to not be used as is, it requires outside scripts to activate its' functions.
// This script exclusively hosts the functions to create, pathfind on and customise the grid.

public class LUX_Grid : MonoBehaviour
{
    // --------------------------------------- Variables ---------------------------------------

    // Private
    private GameController gc;
    private Vector3 x0y0Location = Vector3.zero;
    private GameObject[,] localGridArray;

    // Public
    [Tooltip("This gameobject is what all of the gridbits will be parented to. Cannot be a prefab from inspector. Generally just make this a new empty.")]
    public GameObject gridHolderEmpty;
    [Tooltip("A gridbit can be anything, only thing that matters is that the prefab supplied has the 'LUX_GridBit' script in it.")]
    public GameObject gridBitPrefab;
    [Tooltip("Not set here. No values entered here will work.")]
    public int gridWidth;
    [Tooltip("Not set here. No values entered here will work.")]
    public int gridHeight;

    public Vector2 showcase;
    public bool hitIt;

    // --------------------------------------- Grid Generation Functions ---------------------------------------

    private void Awake()
    {
        gc = gameObject.GetComponent<GameController>();
    }

    public GameObject[,] GridGen(int width, int height)
    {
        GameObject[,] funcReturnVal = new GameObject[width, height];

        for(int tickerA = 0; tickerA < height; tickerA++)
        {
            for(int tickerB = 0; tickerB < width; tickerB++)
            {
                GameObject obj = Instantiate(gridBitPrefab, new Vector3(x0y0Location.x + tickerA, x0y0Location.y, x0y0Location.z + tickerB), Quaternion.identity);
                obj.transform.SetParent(gridHolderEmpty.transform);
                obj.GetComponent<LUX_GridBit>().gridPos.x = tickerA;
                obj.GetComponent<LUX_GridBit>().gridPos.y = tickerB;
                funcReturnVal[tickerA, tickerB] = obj;
                Debug.Log("Generated grid bit, position " + tickerA + ", " + tickerB);
            }
        }

        localGridArray = funcReturnVal;
        return (funcReturnVal);
    }


    // --------------------------------------- Grid Pathfinding Functions ---------------------------------------

    private void SetAllVisitedNegative(Vector2 currentXY)
    {
        foreach(GameObject obj in localGridArray)
        {
            obj.GetComponent<LUX_GridBit>().visited = -1;
        }
        localGridArray[(int)currentXY.x, (int)currentXY.y].GetComponent<LUX_GridBit>().visited = 0;
    }

    private bool TestADirectionFrom(Vector2 fromXY, int step, int direction)
    {
        // This function tests the direction asked of it, by the int direction. The direction chosen is 1 for up, 2 for right and so on clockwise.
        switch (direction)
        {
            case 1: // up
                if(((int)fromXY.y + 1) < gridWidth && localGridArray[(int)fromXY.x,((int)fromXY.y + 1)].GetComponent<LUX_GridBit>().visited == step) {return true;}
                else { return false;}
            case 2: // right
                if (((int)fromXY.x + 1) < gridHeight && localGridArray[(int)fromXY.x + 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().visited == step) { return true;}
                else { return false;}
            case 3: // down
                if (((int)fromXY.y - 1) < gridWidth && localGridArray[(int)fromXY.x, ((int)fromXY.y - 1)].GetComponent<LUX_GridBit>().visited == step) { return true;}
                else { return false;}
            case 4: // left
                if (((int)fromXY.x - 1) < gridWidth && localGridArray[(int)fromXY.x - 1, ((int)fromXY.y)].GetComponent<LUX_GridBit>().visited == step) { return true;}
                else { return false;}
            case 5:
                Debug.Log("You dumbass, you went too high. One to four next time silly.");
                return false;
        }

        Debug.Log("WARNING. TestFourDirectionsFrom() has fucked up.");
        return false; // failsafe
    }

    private void TestAllFourDirections(Vector2 fromXY, int step)
    {
        if (TestADirectionFrom(fromXY, -1, 1))      {SetVisitedVarOnGridBit(new Vector2(fromXY.x, fromXY.y + 1), step);}
        if (TestADirectionFrom(fromXY, -1, 2))      {SetVisitedVarOnGridBit(new Vector2(fromXY.x + 1, fromXY.y), step);}
        if (TestADirectionFrom(fromXY, -1, 3))      {SetVisitedVarOnGridBit(new Vector2(fromXY.x, fromXY.y - 1), step);}
        if (TestADirectionFrom(fromXY, -1, 4))      {SetVisitedVarOnGridBit(new Vector2(fromXY.x - 1, fromXY.y), step);}
    }

    private void SetVisitedVarOnGridBit(Vector2 particularGridbitXY, int step)
    {
        if (localGridArray[(int)particularGridbitXY.x, (int)particularGridbitXY.y])
        {
            localGridArray[(int)particularGridbitXY.x, (int)particularGridbitXY.y].GetComponent<LUX_GridBit>().visited = step;
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
                    TestAllFourDirections(obj.GetComponent<LUX_GridBit>().gridPos, step);
                }
            }
        }
    }

    private void Update()
    {
        if(hitIt == true)
        {
            hitIt= false;

            // set up from showcase vector2
            // wavefront from that shit, baby.

            SetAllVisitedNegative(showcase); // works as is
            SetDistance(showcase); 
        }
    }

    // LUX
}
