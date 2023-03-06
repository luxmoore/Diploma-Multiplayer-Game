using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

        gridComp.GridGen(10, 10); // the dimensions are temporary.
    }

    //private int CheckPlayerAmount()
    //{
    //    foreach (GameObject playerFeller in alivePlayers)
    //    {
    //        if(playerFeller.GetComponent<PlayerStats>().isAlive == true)
    //        {
    //            if(alivePlayers.Contains(playerFeller))
    //            {
    //                // donn do nuthin prettyboy.

    //                // yeah. youse al-redy at da party.
    //            }
    //            else
    //            {
    //                // geddafuck outta here.
                    
    //                // yeah. youse aint welcome rounn here no mores.
    //                alivePlayers.Remove(playerFeller);
    //            }
    //        }
    //    }

    //    maxGoes = alivePlayers.Count;
    //    Debug.Log("Current amount of players: " + maxGoes);

    //    return (alivePlayers.Count);
    //}


    //public void EndGo()
    //{
    //    currentGo++;

    //    if(currentGo >= maxGoes)
    //    {
    //        currentGo = 0;
    //        currentTurn++;
    //    }

    //    currentPlayer = alivePlayers[currentGo];

    //    TurnOverArcher();
    //}

    // LUX
}