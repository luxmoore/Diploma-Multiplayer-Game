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

    [Header("Game Info")]
    [Tooltip("This variable does not change anything, it is just here for debug purposes.")]
    public string gameLobbyName;
    [Tooltip("This variable does not change anything, it is just here for debug purposes.")]
    public bool amHost;

    [Header("Scene Variables")]
    public TextMeshProUGUI lobbyDisplayName;

    [Header("Host Variables")]
    public bool hostReady = false;
    public GameObject hostReadyButtonGameObject;
    public GameObject hostReadyShower;
    public string hostName = "";
    public TextMeshProUGUI hostNameGameObject;
    public Vector2 hostSpawn = new Vector2(-69, -69);
    public TextMeshProUGUI hostSpawnGameObject;

    [Header("Client Variables")]
    public bool clientHere = false;
    public bool clientReady = false;
    public GameObject clientReadyButtonGameObject;
    public GameObject clientReadyShower;
    public string clientName = "";
    public TextMeshProUGUI clientNameGameObject;
    public Vector2 clientSpawn = new Vector2(-69, -69);
    public TextMeshProUGUI clientSpawnGameObject;

    #endregion

    #region Functions

    #region (NESTED) Button Calls

    public void ButtonHostReady()
    {
        HostReady();
    }

    public void ButtonClientReady()
    {
        ClientReady();
    }

    #endregion

    #region (NESTED) Unity Functions

    private void Awake() // essential set up
    {
        Debug.Log("Begginning of AWAKE");
    }

    private void Start() // aux set up
    {
        Debug.Log("Begginning of START");
        gameLobbyName = PhotonNetwork.CurrentRoom.Name;
        SetAppropiateButton();
    }

    private void Update()
    {
        UpdateInfo();

        if(hostReady && clientReady)
        {
            PhotonNetwork.LoadLevel("DualPlayerGameLoop");
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

    private void UpdateInfo()
    {
        // This runs every frame.

        // scene section
        lobbyDisplayName.SetText(gameLobbyName);
        lobbyDisplayName.ForceMeshUpdate();

        // host section
        hostName = PhotonNetwork.PlayerList[0].NickName;
        hostNameGameObject.SetText(hostName);
        hostNameGameObject.ForceMeshUpdate();

        // client section
        if (clientHere)
        {
            clientName = PhotonNetwork.PlayerList[1].NickName;
            clientNameGameObject.SetText(clientName);
            clientNameGameObject.ForceMeshUpdate();
        }
    }

    private void SetAppropiateButton()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            hostReadyButtonGameObject.SetActive(true);
            clientReadyButtonGameObject.SetActive(false);
        }
        else
        {
            hostReadyButtonGameObject.SetActive(false);
            clientReadyButtonGameObject.SetActive(true);
        }

        hostReadyShower.SetActive(false);
        clientReadyShower.SetActive(false);
    }

    #endregion

    #region (NESTED) RPC Functions

    [PunRPC]
    public void HostReady()
    {
        hostReady = true;
        hostReadyButtonGameObject.SetActive(false);
    }

    [PunRPC]
    public void ClientReady()
    {
        clientReady = true;

        clientReadyButtonGameObject.SetActive(false);
    }

    #endregion

    #endregion

    #region Network

    #region (NESTED) Photon Network Functions

    #endregion

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