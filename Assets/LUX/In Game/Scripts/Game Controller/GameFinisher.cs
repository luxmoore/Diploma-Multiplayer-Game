using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFinisher : MonoBehaviour
{
    #region Variables

    public bool debugOut;

    public int deadFellers = 0;
    public int totalFellers = -69;
    public List<int> playerScores;
    public List<int> indexer;

    public GameObject uiCanvasToDisable;
    public GameObject uiCanvasToEnable;

    public TextMeshProUGUI firstName;
    public TextMeshProUGUI firstScore;
    public TextMeshProUGUI secondName;
    public TextMeshProUGUI secondScore;
    public TextMeshProUGUI thirdName;
    public TextMeshProUGUI thirdScore;

    public GameObject uiButtonToEnable;
    GameController gameController;

    private void Start()
    {
        totalFellers = gameObject.GetComponent<GameController>().alivePlayers.Count;
        gameController= gameObject.GetComponent<GameController>();
    }
    #endregion

    public void CheckForEndQualifications()
    {
        if(deadFellers == totalFellers - 1)
        {
            if(debugOut) { Debug.Log("Checked for end qualifications. The game will end now."); }
            StartCoroutine(EndGame());
        }
        else
        {
            if(debugOut) { Debug.Log("Checked for end qualifications. The game will continue."); }
        }
    }

    IEnumerator EndGame()
    {
        // kill all function
        foreach(GameObject feller in gameController.alivePlayers)
        {
            feller.SetActive(false);
        }

        // kill all UI
        uiCanvasToDisable.SetActive(false);

        // calculate all player's "score"
        foreach (GameObject feller in gameController.alivePlayers)
        {
            PlayerStats fellerStats = feller.GetComponentInChildren<PlayerStats>();
            playerScores[fellerStats.playerNum] = fellerStats.totalDamage - fellerStats.totalHealthLost;
        }

        // sort the players scores
        playerScores.Sort();

        // enable screen showing the top 3 players
        uiCanvasToEnable.SetActive(true);

        // change the text to reflect the players

        // after a few seconds enable the continue button
        yield return new WaitForSecondsRealtime(3);
        uiButtonToEnable.SetActive(true);

        yield break;
    }
}
