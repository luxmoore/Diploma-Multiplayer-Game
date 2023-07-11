using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class OnlineSceneManager : MonoBehaviour
{
    #region Variables

    [Header("NonShown Variables")]
    public bool isHostsTurn = true;

    [Header("Game UI")]
    public TextMeshProUGUI turnIndicatorGameObject;
    public GameObject attackButton;

    [Header("Host UI GameObjects")]
    public TextMeshProUGUI hostNameGameObject;
    public TextMeshProUGUI hostHealthGameObject;
    public TextMeshProUGUI hostScoreDamDealtGameObject;
    public TextMeshProUGUI hostScoreDamTakenGameObject;

    [Header("Client UI GameObjects")]
    public TextMeshProUGUI clientNameGameObject;
    public TextMeshProUGUI clientHealthGameObject;
    public TextMeshProUGUI clientScoreDamDealtGameObject;
    public TextMeshProUGUI clientScoreDamTakenGameObject;

    #endregion

    #region Functions

    #region (NESTED) Button Called Functions

    public void ButtonAttack()
    {

    }

    public void ButtonEndTurn()
    {

    }

    #endregion

    #region (NESTED) Unity Functions
    #endregion

    #region (NESTED) Generic Functions

    public void Damage(bool toHost, int howMuch)
    {

    }

    #endregion

    #region (NESTED) Photon RPCs
    #endregion

    #endregion

    #region Photon Network

    #region (NESTED) Photon Network Set Up
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    #endregion

    #region (NESTED) Photon Network Events

    // code index
    public const byte pun_attack = 7;
    public const byte pun_end = 8;

    public void OnEvent(EventData eventData)
    {
        byte eventCode = eventData.Code;
        object[] receivedData = (object[])eventData.CustomData;

        Debug.Log("This client has received an event with the eventNo of " + eventCode);

        switch (eventCode)
        {
            case pun_attack:
                // on sending, tell the other person that they have been attacked for a specified amount of damage.
                // on receiving, take the damage specified.

                int damToTake = (int) receivedData[0];
                Debug.Log("Receiving damage amount : " + damToTake);

                if(PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Damage(true, damToTake);
                }
                else
                {
                    Damage(false, damToTake);
                }

            return;

        }
    }

    #endregion

    #endregion
}
