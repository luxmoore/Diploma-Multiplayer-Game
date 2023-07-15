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
    public int turnamount;

    public OnlinePlayerStats hostStats;
    public OnlinePlayerStats clientStats;

    PhotonView view;

    [Header("Game UI")]
    public TextMeshProUGUI turnIndicatorGameObject;
    public GameObject attackButton;
    public GameObject endGoButton;

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
        view.RPC("EndTurn", RpcTarget.All);
    }

    #endregion

    #region (NESTED) Unity Functions

    private void Start()
    {
        // ---- turn set up ----
        turnamount = 0;

        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            ToggleFunc(true);
        }
        else
        {
            ToggleFunc(false);
        }

        // ---- photon view set up ----
        view = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        PerformFullUIChangeOver();
    }

    #endregion

    #region (NESTED) Generic Functions

    public void Damage(bool isToHost, int howMuch)
    {
        if(isToHost)
        {

        }
    }



    public void ToggleFunc(bool onOrOff)
    {
        if(onOrOff == true)
        {
            // toggle on functionality

            attackButton.gameObject.SetActive(true);
            endGoButton.gameObject.SetActive(true);
        }
        else if(onOrOff == false)
        {
            // toggle off functionality

            attackButton.gameObject.SetActive(false);
            endGoButton.gameObject.SetActive(false);
        }
    }

    private void PerformFullUIChangeOver()
    {
        // Host
        hostNameGameObject.SetText("[H]" + PhotonNetwork.PlayerList[0].NickName);
        hostNameGameObject.ForceMeshUpdate();
        ChangeHealthUI(true, hostStats.playerHealth);
        ChangeDamDealtUI(true, hostStats.scoreDamDealt);
        ChangeDamTakenUI(true, hostStats.scoreDamTaken);

        // Client
        clientNameGameObject.SetText("[C]" + PhotonNetwork.PlayerList[1].NickName);
        clientHealthGameObject.ForceMeshUpdate();
        ChangeHealthUI(false, hostStats.playerHealth);
        ChangeDamDealtUI(false, hostStats.scoreDamDealt);
        ChangeDamTakenUI(false, hostStats.scoreDamTaken);
    }

    private void ChangeHealthUI(bool isToHost, int howMuch)
    {
        TextMeshProUGUI target;

        if(isToHost == true)
        {
            target = hostHealthGameObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            target = clientHealthGameObject.GetComponent<TextMeshProUGUI>();
        }

        target.SetText("HP - " + howMuch + "/100");
        target.ForceMeshUpdate();
    }

    private void ChangeDamDealtUI(bool isToHost, int howMuch)
    {
        TextMeshProUGUI target;

        if(isToHost)
        {
            target = hostScoreDamDealtGameObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            target = clientScoreDamDealtGameObject.GetComponent<TextMeshProUGUI>();
        }

        target.SetText("DD - " + howMuch);
        target.ForceMeshUpdate();
    }

    private void ChangeDamTakenUI(bool isToHost, int howMuch)
    {
        TextMeshProUGUI target;

        if(isToHost == true)
        {
            target = hostScoreDamTakenGameObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            target = clientScoreDamTakenGameObject.GetComponent<TextMeshProUGUI>();
        }

        target.SetText("DT - " + howMuch);
        target.ForceMeshUpdate();
    }

    #endregion

    #region (NESTED) Photon RPCs

    [PunRPC]
    public void EndTurn()
    {
        turnamount = turnamount + 1;

        if(isHostsTurn)
        {
            isHostsTurn = false;
            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                ToggleFunc(false);
            }
            else
            {
                ToggleFunc(true);
            }
        }
        else
        {
            isHostsTurn = true;
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                ToggleFunc(true);
            }
            else
            {
                ToggleFunc(false);
            }
        }
    }

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
