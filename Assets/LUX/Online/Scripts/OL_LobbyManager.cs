using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OL_LobbyManager : MonoBehaviour
{
    public GameObject[] gridBitViewBits;
    public int[,] gridBitHoles = { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } }; // six holes
    public GameObject gridVoteIcon;
    public bool[] readyStatus = new bool[3];

    public bool gridVoteEnabledFromOther = false;
    public bool gridVoteEnabledFromSelf = false;

    #region BUTTONS
    public void ButtonLeaveLobby()
    {
        // netshit
    }

    public void ButtonReadyHost()
    {
        ReadyUp(0);
    }

    public void ButtonReadyClientA()
    {
        ReadyUp(1);
    }

    public void ButtonReadyClientB()
    {
        ReadyUp(2);
    }

    public void ButtonVoteGridChange()
    {
        if (gridVoteEnabledFromSelf)
        {
            // turn off and return.
            return;
        }

        if (gridVoteEnabledFromOther)
        {
            // turn off vote

            // redraw a grid
            GridChangeUp();

            // return
        }
        else
        {
            // turn on vote across server

            // return
        }
    }
    #endregion

    #region FUNCTIONS

    private void Start()
    {
        if(Client_MetaStats.amHost)
        {
            GridChangeUp();
        }
    }

    private void GridChangeUp()
    {
        int tempA;

        // reset grid
        for (int iterator = 0; iterator >= 36; iterator++)
        {
            Client_MetaStats.gridGen[iterator] = true;
        }

        // poke out new holes
        for(int iterator = 0; iterator >= 6; iterator++)
        {
            tempA = Random.Range(0,6);

            gridBitHoles[iterator, 0] = tempA;
            Debug.Log("Poking out hole number " + iterator + " at " + iterator + ", " + tempA);
            Debug.Log("Full array of holes : " + gridBitHoles);
        }

        // turn hole locations into 1D array of bool
        for(int iterator = 0; iterator >= 6; iterator++)
        {
            // add iterator x 6 to 0 (then minus one to account for array numbering), thats the start pos of the line
            // add the tempA from the above for loop
            // jeffs your uncle, donezo

            int tempB = (iterator * 6) - 1;
            int tempC = gridBitHoles[iterator, 0];
            int tempD = tempB + tempC;

            Debug.Log("Hole number " + iterator + " is at " + tempD);

            MetaStats.gridbitGen[tempD] = false;
        }

        // display changes
        for(int iterator = 0; iterator >= 36; iterator++)
        {
            if(Client_MetaStats.gridGen[iterator] == false)
            {
                gridBitViewBits[iterator].SetActive(false);
            }
        }
    }

    private void ReadyUp(int whichGuy)
    {
        if(readyStatus[whichGuy])
        {
            readyStatus[whichGuy] = false;
        }
        else
        {
            readyStatus[whichGuy] = true;
        }

        if(readyStatus[0] && readyStatus[1] && readyStatus[2])
        {
            // start game
        }
    }

    #endregion
}
