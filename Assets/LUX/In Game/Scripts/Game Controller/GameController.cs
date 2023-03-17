using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This script is like the referee, taking in all of the rules of the game and ensuring that all play abides by them. Only holds functions to activate other functions in separate scripts.</summary>

public class GameController : MetaStats
{
    public int[,] startPlayerList; // takes two ints, the first number is the player number and the second number is the player's class.
    public List<GameObject> alivePlayers;
    private GameObject currentPlayer;
    private LUX_TurnSystem turnSystem;
    private LUX_Grid gridComp;

    private void Start()
    {
        turnSystem = gameObject.GetComponent<LUX_TurnSystem>();
        gridComp = gameObject.GetComponent<LUX_Grid>();
        GameSetUp();
    }

    private void GameSetUp()
    {
        // At the start of the game:
        // Generate a grid
        // Place players on that grid
        // Begin turn one, go one - turning on the appropiate player controller

        gridComp.GridGen(gridGenMaxX, gridGenMaxY);
    }

}