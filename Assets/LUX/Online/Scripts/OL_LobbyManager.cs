using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;

public class OL_LobbyManager : MonoBehaviourPunCallbacks, IOnEventCallback, IInRoomCallbacks
{
    public GameObject[] gridBitViewBits;
    public int[,] gridBitHoles = { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } }; // six holes
    public GameObject gridVoteIcon;
    public bool[] readyStatus = { false, false, false };

    public string lobbyMessage;

    [Header("Player Changables")]
    public GameObject[] lobbyMessageBoxes;
    public GameObject[] lobbyPlayerNames;
    public GameObject[] readyButtons;

    public bool gridVoteEnabledFromOther = false;
    public bool gridVoteEnabledFromSelf = false;

    #region Network Event Set Up

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    #endregion

    #region BUTTONS

    public void LobbyMessageUpdate(string message)
    {
        lobbyMessage = message;
    }

    // ----- NETWORKING STUFF FROM HERE -----

    public void ButtonLeaveLobby()
    {
        object[] data = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

        PhotonNetwork.RaiseEvent(playerLeft, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    // the following buttons only exist for the specific client

    [PunRPC]
    public void ButtonReadyHost()
    {
        ReadyUp(0);
    }

    [PunRPC]
    public void ButtonReadyClientA()
    {
        ReadyUp(1);
    }

    [PunRPC]
    public void ButtonReadyClientB()
    {
        ReadyUp(2);
    }

    public void ButtonSendLobbyMessage()
    {
        object[] data = new object[] { lobbyMessage, PhotonNetwork.LocalPlayer.ActorNumber };

        PhotonNetwork.RaiseEvent(netMessage, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
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

        readyButtons[PhotonNetwork.LocalPlayer.ActorNumber].SetActive(true); // activates only the player's ready button
    }

    public override void OnPlayerEnteredRoom(Player newGuy)
    {
        if(Client_MetaStats.amHost)
        {
            Debug.Log("Player has joined and am host, sending them all data on current players");

        }
        else
        {
            Debug.Log("Player has joined, but I dont care, thats the hosts problem.");
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
            } else
            {
                gridBitViewBits[iterator].SetActive(true);
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

    #region Network Functions

    //index of codes
    public const byte playerJoined = 0;
    public const byte netMessage = 1;
    public const byte playerLeft = 2;
    public const byte gridShakeUp = 3;

    // runs when a network event happens
    public void OnEvent(EventData eventData)
    {
        byte eventCode = eventData.Code;
        object[] receivedData = (object[])eventData.CustomData;

        switch(eventCode)
        {
            // event code = 0
            case playerJoined:

                // recieve data
                int playerNum = (int)receivedData[0];
                string playerName = (string)receivedData[1];

                // inform players via the already built in message stuff
                lobbyMessageBoxes[playerNum].GetComponent<TextMeshProUGUI>().SetText("[SERVER] Player has joined.");
                lobbyMessageBoxes[playerNum].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                StartCoroutine(lobbyMessageBoxes[playerNum].GetComponent<TextFader>().FADE());

                // change the name to that of the appropiate player
                lobbyPlayerNames[playerNum].GetComponent<TextMeshProUGUI>().SetText(playerName);

            return;

            // event code = 1
            case netMessage:

                // recieve data
                string msgEventSpeech = (string)receivedData[0];
                int msgEventGuy = (int)receivedData[1];

                // show text and then make it slowly float to under screen
                lobbyMessageBoxes[msgEventGuy].GetComponent<TextMeshProUGUI>().SetText(msgEventSpeech);
                lobbyMessageBoxes[msgEventGuy].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                StartCoroutine(lobbyMessageBoxes[msgEventGuy].GetComponent<TextFader>().FADE());

            return;

            // event code = 2
            case playerLeft:

                // recieve data
                int whoLeft = (int)receivedData[0];

                // inform players via the already built in message stuff
                lobbyMessageBoxes[whoLeft].GetComponent<TextMeshProUGUI>().SetText("[SERVER] Player has left.");
                lobbyMessageBoxes[whoLeft].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                StartCoroutine(lobbyMessageBoxes[whoLeft].GetComponent<TextFader>().FADE());

                // set name to '[SERVER] NOBODY'
                lobbyPlayerNames[whoLeft].GetComponent<TextMeshProUGUI>().SetText("[SERVER] NOBODY");

            return;

            // event code = 3
            case gridShakeUp:

            return;
        }
    }

    #endregion
}
