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
    public List<PlayerStats> playerScores = new List<PlayerStats>();

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
        if (debugOut) { Debug.Log("GameFinisher script has detected a total of " + totalFellers + " in the game."); }
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

    int SortPlayerFunc(PlayerStats a, PlayerStats b) 
    {
        if(a.score < b.score)
        {
            return +1;
        }
        else if(a.score > b.score)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public void AddPlayerScoreToList(PlayerStats data)
    {
        if(playerScores.Contains(data))
        {
            return;
        }
        else
        {
            playerScores.Add(data);
        }
    }

    IEnumerator EndGame()
    {
        // kill all function and calculate all player's "score"
        foreach (GameObject feller in gameController.alivePlayers)
        {
            feller.GetComponent<MeshRenderer>().enabled = false;
            feller.GetComponentInChildren<LUX_GridClicker>().enabled = false;

            PlayerStats fellerStats = feller.GetComponentInChildren<PlayerStats>();
            if (debugOut) { Debug.Log("Adding player number " + fellerStats.playerNum + " who has the name of " + fellerStats.givenName + " who has dealt and received " + fellerStats.totalDamage + "/" + fellerStats.totalHealthLost + " damage."); }
            playerScores.Add(fellerStats);
            fellerStats.score = fellerStats.totalDamage - fellerStats.totalHealthLost;
        }

        // kill all UI
        uiCanvasToDisable.SetActive(false);

        // sort the players scores and change the text to reflect the players
        playerScores.Sort(SortPlayerFunc);

        firstName.SetText(playerScores[0].givenName); firstName.ForceMeshUpdate();
        firstScore.SetText(playerScores[0].score.ToString()); firstScore.ForceMeshUpdate();

        secondName.SetText(playerScores[1].givenName); secondName.ForceMeshUpdate();
        secondScore.SetText(playerScores[1].score.ToString()); secondScore.ForceMeshUpdate();

        thirdName.SetText(playerScores[2].givenName); thirdName.ForceMeshUpdate();
        thirdScore.SetText(playerScores[2].score.ToString()); thirdScore.ForceMeshUpdate();

        // enable screen showing the top 3 players
        uiCanvasToEnable.SetActive(true);

        // after a few seconds enable the continue button
        yield return new WaitForSecondsRealtime(3);
        uiButtonToEnable.SetActive(true);

        yield break;
    }
}
