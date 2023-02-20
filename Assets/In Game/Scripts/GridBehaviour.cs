using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    private int gridWidth;
    private int gridHeight;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottom = new Vector3(0,0,0);
    public GameObject[,] gridArray;
    public int startX, startY;
    public int endX, endY;

    private void Awake()
    {
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
        // You can think of this code as functioning alike one of them old TV scre ens turned on its side:
        // It creates a series of grid spots in a line upwards, until it reaches the height designated.
        // At this point, it moves across by one and repeats the above line. This is repeated until it width designated.
        // The function is then finished.


        for (int tickerA = 0; tickerA < gridHeight; tickerA = tickerA + 1)
        {
            for(int tickerB = 0; tickerB < gridWidth; tickerB = tickerB + 1)
            {
                GameObject surrogateObj = Instantiate(gridPrefab, new Vector3(leftBottom.x + scale * tickerA, leftBottom.y, leftBottom.z + scale * tickerB), Quaternion.identity);
                surrogateObj.transform.SetParent(gameObject.transform);
                surrogateObj.GetComponent<GridStats>().x = tickerB;
                surrogateObj.GetComponent<GridStats>().y = tickerA;
                Debug.Log("Generated grid position " + tickerB + "," + tickerA);
                gridArray[tickerA,tickerB] = surrogateObj;
            }
        }
    }

    //temp
    private void Start()
    {
        GridGeneration(gridWidth, gridHeight);

    }
