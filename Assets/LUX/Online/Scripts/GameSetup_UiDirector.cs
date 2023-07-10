using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameSetup_UiDirector : MonoBehaviourPunCallbacks
{
    [Header("Input Field")]
    public GameObject inputFieldObj;
    public string inputFieldText = null;

    public string textRoomName;
    public string textPlayerName;

    public GameObject onlineMenu;
    public GameObject nameSetMenu;

    public bool createRoom; // this bool determines whether or not you are the host

    public TextMeshProUGUI errorText;

    #region SERVER STUFF

    public void ButtonOnline()
    {
        // this function plays when the user clicks "Online" in the game setup's first menu

        // attempt to connect to master server
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        gameObject.GetComponent<UIDirector>().ChooseOnline();
    }

    #endregion

    #region ROOM CREATION

    public void CreateRoom()
    {
        Client_MetaStats.amHost = true;

        if(IsFilledOut())
        {
            Client_MetaStats.networkCode = inputFieldText;
            PhotonNetwork.CreateRoom(textRoomName);
        }
        else
        {
            ReturnAnError(1);
            return;
        }
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("DualPlayerLobby");
    }

    #endregion

    #region ROOM JOIN

    public void JoinRoom()
    {
        Debug.Log("Attempting to call join. Room name is " + textRoomName);
        if (IsFilledOut())
        {
            Debug.Log("joining");
            Client_MetaStats.networkCode = inputFieldText;
            PhotonNetwork.JoinRoom(textRoomName);
        }
        else
        {
            ReturnAnError(1);
            return;
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        PhotonNetwork.LoadLevel("DualPlayerLobby");
    }

    #endregion

    #region Input Fields

    public void UpdateTheText(string text)
    {
        // this is used both by the room name and player name

        inputFieldText = text;
    }

    private bool IsFilledOut()
    {
        Debug.Log("Checking if the input field is empty.");

        if(inputFieldText != "")
        {
            Debug.Log("The input field is not empty (" + inputFieldText + "). Returning true.");
            return true;
        }
        else
        {
            Debug.Log("The input field is empty. Returning false.");
            return false; 
        }
    }

    #endregion

    #region Buttons

    public void ButtonCreateRoom()
    {
        if (IsFilledOut())
        {
            textRoomName = inputFieldText;

            createRoom = true;

            onlineMenu.SetActive(false);
            nameSetMenu.SetActive(true);
        }
    }

    public void ButtonJoinRoom()
    {
        if (IsFilledOut())
        {
            textRoomName = inputFieldText;

            createRoom = false;

            onlineMenu.SetActive(false);
            nameSetMenu.SetActive(true);
        }
    }

    public void ButtonStart()
    {
        Debug.Log("Attempting to start");
        if (IsFilledOut())
        {
            textPlayerName = inputFieldText;

            PhotonNetwork.LocalPlayer.NickName = textPlayerName;

            if (createRoom)
            {
                CreateRoom();
            }
            else
            {
                Debug.Log("Attempting to join");
                JoinRoom();
            }
        }
    }

    #endregion

    private void ReturnAnError(int errorNum)
    {
        string message = "ERROR NUMBER " + errorNum + ": ";
        errorText.gameObject.SetActive(true);
        switch(errorNum)
        {
            case 1:
                message = message + "No name entered for room.";
            break;

            case 2:
                message = message + "Room does not exist.";
            break;

            case 3:
                message = message + "Room is full.";
            break;

            case 4:
                message = message + "Room already exists.";
            break;
        }

        errorText.SetText(message);
        errorText.ForceMeshUpdate();
    }
}