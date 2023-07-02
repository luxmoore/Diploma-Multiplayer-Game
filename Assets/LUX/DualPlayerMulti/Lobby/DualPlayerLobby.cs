using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;


public class DualPlayerLobby : MonoBehaviour
{
    #region Variables

    [Header("Host Info")]
    public bool hostReady = false;
    public GameObject hostReadyGameObject;
    public string hostName = "";
    public TextMeshProUGUI hostNameGameObject;
    public Vector2 hostSpawn = new Vector2(-69, -69);

    [Header("Client Info")]
    public bool clientReady = false;
    public GameObject clientReadyGameObject;
    public string clientName = "";
    public TextMeshProUGUI clientNameGameObject;
    public Vector2 clientSpawn = new Vector2(-69, -69);

    #endregion

    #region Functions

    #region (NESTED) Button Calls

    public void ButtonHostReady()
    {
        HostReady();

        hostReadyGameObject.SetActive(false);
    }

    public void ButtonClientReady()
    {
        ClientReady();

        clientReadyGameObject.SetActive(false);
    }

    #endregion

    #region (NESTED) Unity Functions

    private void Awake() // essential set up
    {
        
    }

    private void Start() // aux set up
    {
        
    }

    private void Update()
    {
        if(hostReady && clientReady)
        {

        }
    }

    #endregion

    #region (NESTED) Generic Functions

    private void GenerateSpawns(bool forHost)
    {
        int badX; // the other player's X co-ord
        int badY; // the other player's Y co-ord
        if(forHost)
        {
            // host does not need constraint on the spawn position, he always goes first.
            badX = -1;
            badY = -1;
        }
        else
        {
            // the client cannot spawn where the host will.
            badX = (int)hostSpawn.x;
            badY = (int)hostSpawn.y;
        }

        bool stop = false;
        while (stop == false)
        {
            int posX = Random.Range(1, 5);
            int posY = Random.Range(1, 5);

            if( posX != badX && posY != badY ) // (client) if the position generated is not the host's...
            {
                stop = true; // stop the generator. Else, continue.

                if (forHost)
                {
                    hostSpawn = new Vector2(posX, posY);
                }
                else
                {
                    clientSpawn = new Vector2(posX, posY);
                }
            }
        }
    }

    #endregion

    #region (NESTED) RPC Functions

    [PunRPC]
    public void HostReady()
    {
        hostReady = true;
    }

    [PunRPC]
    public void ClientReady()
    {
        clientReady = true;
    }

    #endregion

    #endregion

    #region Network

    #region (NESTED) Network Event Set Up

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    #endregion

    #region (NESTED) Network Events
    public void OnEvent(EventData eventData)
    {
        byte eventCode = eventData.Code;
        object[] receivedData = (object[])eventData.CustomData;

        Debug.Log("This client has received an event with the eventNo of " + eventCode);

    }
    #endregion

    #endregion
}