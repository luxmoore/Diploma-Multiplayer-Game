using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;

public class OL_PlayerFunc : MonoBehaviour
{
    #region Variables

    public TextMeshProUGUI playerName;
    public OL_GameController gameController;

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

    #region Set Up

    // game set up is done during Awake in OL_GameController.cs

    void Start() // online player set up
    {
        Debug.Log("Player number " + PhotonNetwork.LocalPlayer.ActorNumber + " has joined");

        // place the player on the grid
        gameObject.transform.position = new Vector3(Client_MetaStats.myLocation[0], 1, Client_MetaStats.myLocation[1]);

        // set the name of the player
        playerName.SetText(PhotonNetwork.LocalPlayer.NickName);
        playerName.ForceMeshUpdate();

        // tell the master client that we are here if we are not the master client
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) // if i am not the master client,
        {
            PhotonNetwork.RaiseEvent(playerIn, null, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    #endregion

    #region Network Events

    // index of codes
    public const byte playerIn = 7;

    public void OnEvent(EventData eventData)
    {
        byte eventCode = eventData.Code;
        object[] receivedData = (object[])eventData.CustomData;

        Debug.Log("This client has received an event with the eventNo of " + eventCode);

        switch(eventCode)
        {
            case playerIn:
                // when sending, client informs the host that they are in - without sending in any data.
                // when receiving, 

            break;
        }
    }

    #endregion
}
