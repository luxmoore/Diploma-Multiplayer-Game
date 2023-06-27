using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;

public class OL_LobbyManager : MonoBehaviourPunCallbacks, IOnEventCallback, IInRoomCallbacks
{
    #region Variables

    public GameObject[] gridBitViewBits;
    public int[,] gridBitHoles = { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } }; // six holes
    public GameObject gridVoteIcon;

    public string lobbyMessage;

    [Header("Player Changables")]
    public GameObject[] lobbyMessageBoxes;
    public GameObject[] lobbyPlayerNames;
    public GameObject[] readyButtons;
    public GameObject[] readyDisplayer;
    public bool[] whoHere = { true, false, false };
    public string[] playerNamesStringArray = { "host", "client1", "client2" };
    public bool[] readyStatus = { false, false, false };

    public bool gridVoteEnabledFromOther = false;
    public bool gridVoteEnabledFromSelf = false;

    [Header("Debug Vars")]
    public int myActorNum;
    public int myArrayNum;
    public string networkCode;

    #endregion

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
    public void ReadyUpButtonFunction(int playerNum)
    {
        GetComponent<PhotonView>().RPC("ButtonReady", RpcTarget.All, playerNum);
    }

    [PunRPC]
    void ButtonReady(int playernum)
    {
        ReadyUp(playernum);
    }

    public void ButtonSendLobbyMessage()
    {
        object[] data = new object[] { lobbyMessage, PhotonNetwork.LocalPlayer.ActorNumber };

        PhotonNetwork.RaiseEvent(netMessage, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
     
    public void ButtonVoteGridChange() // unimplemented in current plan
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
        else
        {
            // send and recieve all data about players currently in lobby
            // add self to all current player's data

            object[] dataSent = { PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName };
            PhotonNetwork.RaiseEvent(playerJoined, dataSent, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        }

        playerNamesStringArray[PhotonNetwork.LocalPlayer.ActorNumber - 1] = PhotonNetwork.LocalPlayer.NickName; // add self to the player names variable

        readyButtons[PhotonNetwork.LocalPlayer.ActorNumber - 1].SetActive(true); // activates only the player's ready button

        UpdateAllInfo();

        // these are just used to see in inspector
        myActorNum = PhotonNetwork.LocalPlayer.ActorNumber;
        myArrayNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        //networkCode = PhotonNetwork.
    }

    private void Update()
    {
        if (readyStatus[0] && readyStatus[1] && readyStatus[2])
        {
            // move to scene 7
            PhotonNetwork.LoadLevel("OnlineGameLoop");
        }
    }

    public override void OnPlayerEnteredRoom(Player newGuy)
    {
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("Player has joined and am host, sending them all data on current players");

            // get a refresh on all info
            PhotonNetwork.RaiseEvent(demandAllInfo, null, RaiseEventOptions.Default, SendOptions.SendUnreliable);

            // send all updated data to the player that joined
            object[] dataSent = { whoHere, lobbyPlayerNames, readyButtons };
            PhotonNetwork.RaiseEvent(sendAllInfo, dataSent, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        }
        else
        {
            Debug.Log("Player has joined, but I dont care, thats the hosts problem.");
            UpdateAllInfo();
        }
    }

    #region Grid Stuff
    private void GridChangeUp()
    {
        int tempA;

        // reset grid
        for (int iterator = 0; iterator >= 36; iterator++)
        {
            Client_MetaStats.gridGen[iterator] = true;
        }

        // poke out new holes
        for (int iterator = 0; iterator >= 6; iterator++)
        {
            tempA = Random.Range(0, 6);

            gridBitHoles[iterator, 0] = tempA;
            Debug.Log("Poking out hole number " + iterator + " at " + iterator + ", " + tempA);
            Debug.Log("Full array of holes : " + gridBitHoles);
        }

        // turn hole locations into 1D array of bool
        for (int iterator = 0; iterator >= 6; iterator++)
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
        for (int iterator = 0; iterator >= 36; iterator++)
        {
            if (Client_MetaStats.gridGen[iterator] == false)
            {
                gridBitViewBits[iterator].SetActive(false);
            }
            else
            {
                gridBitViewBits[iterator].SetActive(true);
            }
        }
    }
    #endregion

    private void UpdateAllInfo() // changes all of the information displayed to be accurate to last recieved data
    {
        Debug.Log("UpdateAllInfo running");

        for(int i = 0; i < 3; i++)
        {
            Debug.Log("UpdateAllInfo run number: " + i);

            if(PhotonNetwork.CurrentRoom.Players.Count > i) // if they are in the game
            {
                lobbyPlayerNames[i].GetComponent<TextMeshProUGUI>().SetText(PhotonNetwork.CurrentRoom.Players[i].NickName);
                lobbyPlayerNames[i].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();

                if(readyStatus[i]) // if they are ready
                {
                    readyDisplayer[i].SetActive(true);
                }
                else
                {
                    readyDisplayer[i].SetActive(false);
                }
            }
            else
            {
                lobbyPlayerNames[i].GetComponent<TextMeshProUGUI>().SetText("[SERVER] NOBODY");
                lobbyPlayerNames[i].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();

                readyDisplayer[i].SetActive(false);
            }
        }
    }

    private void ReadyUp(int whichGuy)
    {
        Debug.Log("Player number " + whichGuy + " has readied up");

        if(readyStatus[whichGuy])
        {
            readyStatus[whichGuy] = false;
        }
        else
        {
            readyStatus[whichGuy] = true;
        }
        
        // startgame check done in update loop

    }

    #endregion

    #region Network Functions

    //index of codes
    public const byte playerJoined = 0;
    public const byte netMessage = 1;
    public const byte playerLeft = 2;
    public const byte gridShakeUp = 3;
    public const byte demandAllInfo = 4;
    public const byte sendAllInfo = 5;
    public const byte sendSelfInfoToHost = 6;

    // runs when a network event happens
    public void OnEvent(EventData eventData)
    {
        byte eventCode = eventData.Code;
        object[] receivedData = (object[])eventData.CustomData;

        Debug.Log("This client has received an event with the eventNo of " + eventCode);

        switch(eventCode)
        {
            #region event code = 0
            case playerJoined:

                // when sending,
                // when receiving, 

                // recieve data
                int playerNum = (int)receivedData[0];
                string playerName = (string)receivedData[1];

                // inform players via the already built in message stuff
                lobbyMessageBoxes[playerNum - 1].GetComponent<TextMeshProUGUI>().SetText("[SERVER] Player has joined.");
                lobbyMessageBoxes[playerNum - 1].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                StartCoroutine(lobbyMessageBoxes[playerNum - 1].GetComponent<TextFader>().FADE());

                // change the name to that of the appropiate player
                lobbyPlayerNames[playerNum - 1].GetComponent<TextMeshProUGUI>().SetText(playerName);

                // set the arrays to contain player data

            return;
            #endregion

            #region event code = 1
            case netMessage:

                // when sending,
                // when receiving, 

                // recieve data
                string msgEventSpeech = (string)receivedData[0];
                int msgEventGuy = (int)receivedData[1];

                // show text and then make it slowly float to under screen
                lobbyMessageBoxes[msgEventGuy - 1].GetComponent<TextMeshProUGUI>().SetText(msgEventSpeech);
                lobbyMessageBoxes[msgEventGuy - 1].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                StartCoroutine(lobbyMessageBoxes[msgEventGuy - 1].GetComponent<TextFader>().FADE());

            return;
            #endregion

            #region event code = 2
            case playerLeft:

                // when sending, client tells the host that they have left the game
                // when recieving, the host is informed of a player leaving and displays for all.

                // recieve data
                int whoLeft = (int)receivedData[0];

                // inform players via the already built in message stuff
                lobbyMessageBoxes[whoLeft - 1].GetComponent<TextMeshProUGUI>().SetText("[SERVER] Player has left.");
                lobbyMessageBoxes[whoLeft - 1].GetComponent<TextMeshProUGUI>().ForceMeshUpdate();
                StartCoroutine(lobbyMessageBoxes[whoLeft - 1].GetComponent<TextFader>().FADE());

                // set name to '[SERVER] NOBODY'
                lobbyPlayerNames[whoLeft - 1].GetComponent<TextMeshProUGUI>().SetText("[SERVER] NOBODY");

            return;
            #endregion

            #region event code = 3
            case gridShakeUp:

                // too ambitious

            return;
            #endregion

            #region event code = 4
            case demandAllInfo:

                // when sending, the host gets all the info on all of the players
                // when recieving, the client gives the host all of the info that they have on themselves via a function

                if(PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    return;
                }
                else
                {
                    object[] sendData = { PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName, readyStatus[PhotonNetwork.LocalPlayer.ActorNumber] };
                    PhotonNetwork.RaiseEvent(sendSelfInfoToHost, sendData, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                }

            return;
            #endregion

            #region event code = 5
            case sendAllInfo:

                // when sending, the host sends all the info on all of the players
                // when receiving, the client receives all of the information on all other players in the room currently, including the host

                whoHere = (bool[])receivedData[0];
                playerNamesStringArray = (string[])receivedData[1];
                readyStatus = (bool[])receivedData[2];



            return;
            #endregion

            #region event code = 6
            case sendSelfInfoToHost:

                // when sending, the client gives all the info they have on themselves
                // when receiving, check if am host, if not, forget about about it, if am, add the data to the arrays

                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    playerNamesStringArray[(int)receivedData[0] - 1] = (string)receivedData[1];
                    readyStatus[(int)receivedData[0] - 1] = (bool)receivedData[2];
                }
                else
                {
                    return;
                }

                return;
            #endregion
        }
    }

    #endregion
}