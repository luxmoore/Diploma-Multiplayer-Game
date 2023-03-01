using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is designed to not be used as is, it requires the GameController component to function at all.
// This script exclusively hosts the functions to create, pathfind on and customise the grid.

public class LUX_Grid : MonoBehaviour
{
    // --------------------------------------- Variables ---------------------------------------

    // Private
    private GameController gc;
    private Vector3 x0y0Location = Vector3.zero;
    private GameObject[,] localGridArray;

    // Public
    public GameObject gridHolderEmpty;
    public GameObject gridBitPrefab;


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
                funcReturnVal[tickerA, tickerB] = obj;
                Debug.Log("Generated grid bit, position " + tickerA + ", " + tickerB);
            }
        }

        localGridArray = funcReturnVal;
        return (funcReturnVal);
    }


    // --------------------------------------- Grid Pathfinding Functions ---------------------------------------

    public void PathFind(Vector2 gridStartXY, int maxSteps, Vector2 gridEndXY)
    {
        GenerateSteps(gridStartXY, maxSteps);
        SetPath();
    }

    public void GenerateSteps(Vector2 gridStartXY, int maxSteps)
    {
        // This function steps out each possible move, setting a bool on each tile possible to true, meaning that it can be moved to.

    }

    private void SetPath()
    {
        Debug.Log("yo mama");
    }

    private void TestFourDirectionsFrom(Vector2 startXY)
    {

    }
    // LUX
}
