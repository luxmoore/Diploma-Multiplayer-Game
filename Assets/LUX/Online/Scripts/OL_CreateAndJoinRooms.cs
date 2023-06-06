using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class OL_CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    #region Fields
    public TMP_InputField joinRoomName;
    public TMP_InputField createRoomName;
    public TMP_Text errorLog;
    public byte maxPlayersPerRoom = 3; // max players
    #endregion

    #region Public Functions
    public void ButtonConnectedJoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomName.text);
    }

    public void ButtonConnectedCreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomName.text, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public void ButtonConnectedBackToMain()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Pun Callbacks
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        string errorMessage = "Failed to join room. Error - " + message;
        Debug.Log(errorMessage);
        errorLog.text = errorMessage;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Waiting");
    }
    #endregion
}
