using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class LUX_PHOTON_Launcher : MonoBehaviourPunCallbacks
{
    #region Serializable Fields
    #endregion

    #region Private Fields
    private string gameVersion = "0.5.4.3b0.2"; // Client's version number
    #endregion

    #region MonoBehaviour Callbacks

    // On Awake, ensure that everybody's scenes are synced when PhotonNetwork.LoadLevel() is called
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // CRITICAL LINE OF CODE! DO NOT ALTER!
    }

    // On Start, attempt a connection to the master server
    private void Start()
    {
        Debug.Log("Attempting to connect to master server");
        Connect();
    }
    #endregion

    #region Public Methods
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("1");
            PhotonNetwork.JoinLobby();
        }
        else
        {
            Debug.Log("2");
            PhotonNetwork.ConnectUsingSettings(); // if successful, runs OnConnectedToMaster(), hence why we override it in the Pun Callbacks region
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    #endregion

    #region Pun Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to a server. Attempting to join a lobby");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Failed to connect. Disconnected was called with the reason: " + cause);
        SceneManager.LoadScene(0);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby joined. Starting scene: CreateOrJoin");
        PhotonNetwork.LoadLevel("CreateOrJoin");
    }
    #endregion
}