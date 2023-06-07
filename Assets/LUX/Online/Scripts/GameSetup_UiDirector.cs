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

    public TextMeshProUGUI errorText;

    #region ROOM CREATION

    public void ButtonCreateRoom()
    {
        if(IsFilledOut())
        {
            PhotonNetwork.CreateRoom(inputFieldText);
        }
        else
        {
            ReturnAnError(1);
            return;
        }
    }

    #endregion

    #region ROOM JOIN

    public void ButtonJoinRoom()
    {
        if (IsFilledOut())
        {
            PhotonNetwork.JoinRoom(inputFieldText);
        }
        else
        {
            ReturnAnError(1);
            return;
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    #endregion

    #region Input Field

    public void UpdateTheText(string text)
    {
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