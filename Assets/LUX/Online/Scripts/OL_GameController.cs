using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OL_GameController : MonoBehaviour
{
    #region Variables
    // prefabs
    public GameObject playerPrefab;

    // arrays
    public GameObject[] players;

    // non-online
    public LUX_Grid gridComp;

    #endregion

    #region Game Set Up
    private void Awake()
    {
        
    }

    #endregion
}
