using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUX_Grid : MonoBehaviour
{
    // --------------------------------------- Variables ---------------------------------------

    // Private
    private GameController gc;
    private Vector3 x0y0Location = Vector3.zero;

    // Public
    public GameObject gridHolderEmpty;
    public GameObject gridBitPrefab;


    // --------------------------------------- Functions ---------------------------------------


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

            }
        }

        return (funcReturnVal);
    }

    // LUX
}
