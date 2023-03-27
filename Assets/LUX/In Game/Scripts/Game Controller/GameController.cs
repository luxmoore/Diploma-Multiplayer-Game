using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This script is like the referee, taking in all of the rules of the game and ensuring that all play abides by them. Only holds functions to activate other functions in separate scripts.</summary>

public class GameController : MetaStats
{
    public int[,] startPlayerList = new int[1,1]; // takes two ints, the first number is the player number and the second number is the player's class. Current values are temporary
    public List<GameObject> alivePlayers;
    private LUX_TurnSystem turnSystem;
    private LUX_Grid gridComp;
    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        for(int ticker = 0; ticker < startPlayerList.GetLength(0); ticker ++) // the getlength() function works specifically for multidimensional arrays
        {
            // first create the correct amount of player prefabs, then assign variables to them.
            GameObject playerPre = Instantiate(playerPrefab);

            // PlayerController variables
            PlayerStats playerPreContr = playerPre.GetComponentInChildren<PlayerStats>();
            playerPreContr.playerNum = ticker + 1;
            playerPreContr.isAlive = true;

            alivePlayers.Add(playerPre);
        }
    }

    private void Start()
    {
        turnSystem = gameObject.GetComponent<LUX_TurnSystem>();
        gridComp = gameObject.GetComponent<LUX_Grid>();
        GameSetUp();
    }

    private void GameSetUp()
    {
        // At the start of the game:
        // Generate a grid (the function also places players on that grid)
        // Begin turn one, go one - turning on the appropiate player controller

        gridComp.GridGen(gridGenMaxX, gridGenMaxY, gridHolesAmount);
    }
}