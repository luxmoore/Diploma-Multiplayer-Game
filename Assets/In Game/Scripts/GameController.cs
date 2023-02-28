using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MetaStats
{
    public int[,] startPlayerList; // takes two ints, the first number is the player number and the second number is the player's class.
    public List<GameObject> alivePlayers;
    public GameObject[,] gridBits; // stores all gridbits
    public List<GameObject> walkableGridBits; // stores all walkable gridbits, is not used for anything except checking against
    public int currentGo = 0;
    public int maxGoes = 0;
    public int currentTurn;
    public bool playerInfluencable = false;
    public bool gameGoing = true;
    private GameObject currentPlayer;
    private Scene scene;

    private void Start()
    {
        GameSetUp();
    }

    private void GameSetUp()
    {
        GenerateGrid();
        GeneratePlayers();
        CheckPlayerAmount();
        TurnOverArcher();
        currentGo = 0;
        currentTurn = 1;

    }

    private void TurnOverArcher()
    {
        StartGo(); // makes the last playercontroller inactive and turns on the next player controller
        BackToGameControllerForBaking(); // bakes the list of actions that the player undertook, then starts a coroutine in the player's feller to undertake all those actions.
                                         // the playercontroller has no relation to the feller until its inputs are sent back to the G.C.
        EndGo(); // increments the go by one, changing the turn if the go amount exceeds the amount of players, then restarts the 'turn over-archer' function.
    }

    private int CheckPlayerAmount()
    {
        foreach (GameObject thinger in alivePlayers)
        {
            if(thinger.GetComponent<PlayerStats>().isAlive == true)
            {
                if(alivePlayers.Contains(thinger))
                {
                    // donn do nuthin prettyboy.

                    // yeah. youse al-redy at da party.
                }
                else
                {
                    // geddafuck outta here.
                    alivePlayers.Remove(thinger);
                }
            }
        }

        maxGoes = alivePlayers.Count;
        Debug.Log("Current amount of players: " + maxGoes);

        return (alivePlayers.Count);
    }

    private void GeneratePlayers()
    {

    }

    private void GenerateGrid()
    {
        gridBits = gameObject.GetComponent<LUX_Grid>().GridGen(gridGenMaxX, gridGenMaxY);

    }

    private void StartGo()
    {

    }

    private void BackToGameControllerForBaking()
    {

    }

    public void EndGo()
    {
        currentGo++;

        if(currentGo == maxGoes)
        {
            currentGo = 0;
            currentTurn++;
        }

        currentPlayer = alivePlayers[currentGo];

        TurnOverArcher();
    }

    // LUX
}