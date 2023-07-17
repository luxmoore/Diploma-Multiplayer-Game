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
    private int tempDamVal;
    public OnlinePlayerStats hostStats;
    public OnlinePlayerStats clientStats;
    PhotonView view;

    [Header("Game UI")]
    public TextMeshProUGUI turnIndicatorGameObject;
    public GameObject attackButton;
    public GameObject endGoButton;
    public int currentEnergy = 3;
    public TextMeshProUGUI energyIndicator;

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
        // minus one energy
        currentEnergy = currentEnergy - 1;

        // if the player has energy...
        if( currentEnergy > 0)
        {
            // generate random number
            tempDamVal = RandGenNum();
            Debug.Log("Damage generated as " + tempDamVal);

            // send that to the host to sync numbers
            object[] data = new object[] { tempDamVal };
            PhotonNetwork.RaiseEvent(pun_attack, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
        else //...
        {
            // set current energy to zero
            currentEnergy = 0;

            // do nuthin

        }
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

    public void ToggleFunc(bool onOrOff)
    {
        currentEnergy = 3;

        if(onOrOff == true)
        {
            // toggle on functionality

            attackButton.gameObject.SetActive(true);
            endGoButton.gameObject.SetActive(true);
            energyIndicator.gameObject.SetActive(true);
        }
        else if(onOrOff == false)
        {
            // toggle off functionality

            attackButton.gameObject.SetActive(false);
            endGoButton.gameObject.SetActive(false);
            energyIndicator.gameObject.SetActive(false);
        }
    }

    private int RandGenNum()
    {
        if(isHostsTurn)
        {
            return Random.Range(hostStats.minDam, hostStats.maxDam);
        }
        else
        {
            return Random.Range(clientStats.minDam, clientStats.maxDam);
        }
    }

    private void PerformFullUIChangeOver()
    {
        if(isHostsTurn)
        {
            turnIndicatorGameObject.SetText("[H] Turn " + turnamount);
        }
        else
        {
            turnIndicatorGameObject.SetText("[C] Turn " + turnamount);
        }

        ChangeEnergyUI();

        // Host
        hostNameGameObject.SetText("[H]" + PhotonNetwork.PlayerList[0].NickName);
        hostNameGameObject.ForceMeshUpdate();
        ChangeHealthUI(true, hostStats.playerHealth);
        ChangeDamDealtUI(true, hostStats.scoreDamDealt);
        ChangeDamTakenUI(true, hostStats.scoreDamTaken);

        // Client
        clientNameGameObject.SetText("[C]" + PhotonNetwork.PlayerList[1].NickName);
        clientHealthGameObject.ForceMeshUpdate();
        ChangeHealthUI(false, clientStats.playerHealth);
        ChangeDamDealtUI(false, clientStats.scoreDamDealt);
        ChangeDamTakenUI(false, clientStats.scoreDamTaken);
    }

    private void ChangeEnergyUI()
    {
        energyIndicator.SetText("Energy - " + currentEnergy);
        energyIndicator.ForceMeshUpdate();
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

    [PunRPC]
    public void Damage(bool isToHost, int howMuch)
    {
        if (isToHost)
        {
            hostStats.playerHealth = hostStats.playerHealth - howMuch;
            hostStats.scoreDamTaken = hostStats.scoreDamTaken + howMuch;

            clientStats.scoreDamDealt = clientStats.scoreDamDealt + howMuch;

            if(hostStats.playerHealth <= 0)
            {
                view.RPC("EndGame", RpcTarget.All, false);
            }
        }
        else
        {
            clientStats.playerHealth = clientStats.playerHealth - howMuch;
            clientStats.scoreDamTaken = clientStats.scoreDamTaken + howMuch;

            hostStats.scoreDamDealt = hostStats.scoreDamDealt + howMuch;

            if (clientStats.playerHealth <= 0)
            {
                view.RPC("EndGame", RpcTarget.All, true);
            }
        }
    }

    [PunRPC]
    public void EndGame(bool hostWon)
    {

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

                // turn received data into the tempDamVal int
                tempDamVal = (int)receivedData[0];
                Debug.Log("Damage variable recieved as " + tempDamVal);

                // RPC that shit
                if (isHostsTurn)
                {
                    view.RPC("Damage", RpcTarget.All, false, tempDamVal);
                }
                else
                {
                    view.RPC("Damage", RpcTarget.All, false, tempDamVal);
                }

            return;
        }
    }

    #endregion

    #endregion
}
