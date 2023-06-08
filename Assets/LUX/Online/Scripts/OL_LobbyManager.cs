using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OL_LobbyManager : MonoBehaviour
{
    public GameObject[] gridBitViewBits;
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

        }
        else
        {

        }
    }
    #endregion

    private void Update()
    {
        // refresh the grid if neccessary. On refresh, reset both gridVoteEnabled bools
        
    }

    #region FUNCTIONS

    private void GridChangeUp()
    {

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
