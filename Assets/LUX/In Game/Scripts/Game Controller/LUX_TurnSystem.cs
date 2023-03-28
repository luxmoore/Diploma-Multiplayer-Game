using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUX_TurnSystem : MonoBehaviour
{
    // --------------------------------------- Variables ---------------------------------------

    private GameController gameController;
    private LUX_Grid gridComp;
    private FollowingText followingText;

    [Header("Miscellanious Variables")]
    public List<GameObject> playerEntities;
    public int playerAmount;
    public int playerAmountAlive;
    public int currentTurn = 1;
    public int whoseGo = 0;
    public bool goEnded = true;
    public bool printResults;

    //[Header("State Machine Variables")]
    public enum STATES {TURN, LIMBO}
    public STATES currentState = STATES.TURN;

    // --------------------------------------- Functions --------------------------------------- 

    private void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
        gridComp = gameController.GetComponent<LUX_Grid>();

        playerEntities = gameController.alivePlayers;
        playerAmountAlive = playerEntities.Count;

        for(int ticker = 0; ticker >= playerAmountAlive; ticker++)
        {
            ToggleFunc(ticker, false);
        }

        followingText = GameObject.FindWithTag("FollowingText").GetComponent<FollowingText>();
    }

    /// <summary>
    /// Use the second param to turn on (true) or off (false) the functionality of the player specified in the first param
    /// </summary>
    private void ToggleFunc(int playerNum, bool onOff)
    {
        playerEntities[playerNum].GetComponentInChildren<LUX_GridClicker>().enabled = onOff;

        Debug.Log("Player " + playerNum + "'s functionality is set to " + onOff + ".");
        followingText.UpdateText("");
    }

    // --------------------------------------- Coroutines --------------------------------------- 

    public IEnumerator TURN()
    {
        // set an int as being the amount of players that are currently alive.
        // this int will be used as a way to start and end the turn

        while(true)
        {
            if(goEnded == true)
            {
                ToggleFunc(whoseGo, false);
                Debug.Log("Player number " + whoseGo + "has ended their turn.");

                goEnded = false;
                playerEntities = gameController.alivePlayers;
                playerAmountAlive = playerEntities.Count;

                if(whoseGo != (playerAmountAlive - 1)) // if the maximum amount of player goes is not reached for the turn
                {
                    // whosego might not function like this. Because the player is selected from the list by the whosego int,
                    // but the list may have have more people than the int, making every player after that miss out on their turn.
                    // I can avoid this by updating the list using the .remove function on the player's death.
                    whoseGo++;
                }
                else
                {
                    whoseGo = 0;
                    currentTurn++;
                }

                if(printResults == true)
                {
                    Debug.Log("At the end of the turn:");
                    foreach(GameObject obj in playerEntities)
                    {
                        Debug.Log("");
                        PlayerStats jimbo = obj.GetComponentInChildren<PlayerStats>();
                        Debug.Log("Player number " + jimbo.playerNum + ", or, " + jimbo.givenName + ", has " + jimbo.currentHealth + "/" + jimbo.maxHealth + " health");
                        Debug.Log("This player has (in total) dealt " + jimbo.totalDamage + " damage and taken " + jimbo.totalHealthLost);
                    }
                }

                Go();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void Go()
    {
        ToggleFunc(whoseGo, true);

        PlayerStats currentStats = playerEntities[whoseGo].GetComponentInChildren<PlayerStats>();
        gridComp.SetUpGo(currentStats.gridPos);
    }

    // LUX
}