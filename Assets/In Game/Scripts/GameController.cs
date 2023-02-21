using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] players; // remember, arrays start at zero
    public int currentGo = 0;
    public int maxGoes = 0;
    public int currentTurn;
    private GameObject currentPlayer;

    private void Start()
    {
        maxGoes = players.Length;
        currentGo = 0;
        currentTurn = 1;

        Debug.Log("Current amount of players: " + maxGoes);

        GameSetUp();
    }

    private void GameSetUp()
    {

    }

    public void TurnEnd()
    {
        currentGo++;

        if(currentGo == maxGoes)
        {
            currentGo = 0;
            currentTurn++;
        }

        currentPlayer = players[currentGo];
    }
}
