using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LUX_TurnSystem : MonoBehaviour
{
    #region Variables

    private GameController gameController;
    private LUX_Grid gridComp;
    private FollowingText followingText;
    private TextMeshProUGUI movementEnergyDisplay;
    private TextMeshProUGUI attackEnergyDisplay;
    private TextMeshProUGUI healthDisplay;
    private TextMeshProUGUI turnDisplay;
    private TextMeshProUGUI goDisplay;

    [Header("Miscellanious Variables")]
    public List<GameObject> playerEntities;
    public int playerAmount;
    public int playerAmountAlive;
    public int currentTurn = 1;
    public int whoseGo = 0;
    public bool goEnded = false;
    public bool printResults;

    #endregion

    #region Functions 

    private void Start()
    {
        goEnded = false; // this wont stop being true on start unless I do this. no clue why.

        gameController = gameObject.GetComponent<GameController>();
        gridComp = gameController.GetComponent<LUX_Grid>();

        playerEntities = gameController.alivePlayers;
        playerAmountAlive = playerEntities.Count;

        playerAmount = playerAmountAlive;

        followingText = GameObject.FindWithTag("FollowingText").GetComponent<FollowingText>();


        for (int ticker = 0; ticker < playerAmountAlive; ticker++) // this turns off everybody's functionality.
        {
            ToggleFunc(ticker, false);
            Debug.Log("Player number " + ticker + " has had their functionality removed for initialization.");
        }

        movementEnergyDisplay = GameObject.FindGameObjectWithTag("MovementText").GetComponent<TextMeshProUGUI>();
        attackEnergyDisplay = GameObject.FindGameObjectWithTag("AttackText").GetComponent<TextMeshProUGUI>();
        healthDisplay = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();

        turnDisplay = GameObject.FindGameObjectWithTag("TurnText").GetComponent<TextMeshProUGUI>();
        goDisplay = GameObject.FindGameObjectWithTag("GoText").GetComponent<TextMeshProUGUI>();

        StartCoroutine("TURN");
        FirstGoChooser();
    }

    /// <summary>
    /// Use the second param to turn on (true) or off (false) the functionality of the player specified in the first param
    /// </summary>
    public void ToggleFunc(int playerNum, bool onOff)
    {
        playerEntities[playerNum].GetComponentInChildren<LUX_GridClicker>().enabled = onOff;

        Debug.Log("Player " + playerNum + "'s functionality is set to " + onOff + ".");
        followingText.UpdateText("");
    }

    public void RecieveEndGoFromButton()
    {
        Debug.Log("Request to end go recieved.");
        goEnded = true;
    }

    #region Change UI Text
    private void ChangeStatText(int movementEnergy, int attackEnergy, int currentHealth, int maxHealth)
    {
        movementEnergyDisplay.SetText("MVM - " + movementEnergy.ToString());
        movementEnergyDisplay.ForceMeshUpdate();

        attackEnergyDisplay.SetText("ATK - " + attackEnergy.ToString());
        attackEnergyDisplay.ForceMeshUpdate();

        healthDisplay.SetText("HTP - " + currentHealth + "/" + maxHealth);
        healthDisplay.ForceMeshUpdate();
    }

    private void ChangeTurnText(int turnAmount, int goAmount)
    {
        turnDisplay.SetText("Turn " + turnAmount);
        turnDisplay.ForceMeshUpdate();

        goDisplay.SetText("Go " + goAmount);
        goDisplay.ForceMeshUpdate();
    }
    #endregion
    #endregion

    #region Turn System

    public IEnumerator TURN()
    {
        // set an int as being the amount of players that are currently alive.
        // this int will be used as a way to start and end the turn

        while(true)
        {
            if(goEnded == true) // this first runs on the end of first go
            {
                ToggleFunc(whoseGo, false);
                Debug.Log("Player number " + whoseGo + "has ended their turn.");

                
                playerEntities = gameController.alivePlayers;

                if (printResults == true)
                {
                    Debug.Log("At the end of the turn:");
                    foreach (GameObject obj in playerEntities)
                    {
                        Debug.Log("");
                        PlayerStats jimbo = obj.GetComponentInChildren<PlayerStats>();
                        Debug.Log("Player number " + jimbo.playerNum + ", or, " + jimbo.givenName + ", has " + jimbo.currentHealth + "/" + jimbo.maxHealth + " health");
                        Debug.Log("This player has (in total) dealt " + jimbo.totalDamage + " damage and taken " + jimbo.totalHealthLost);
                    }
                }

                #region Go Exchange

                // this for loop checks for the next available feller.
                // after choosing the next feller, it changes the 'whoseGo' variable to that
                // if the next feller is impossible without returning an out of bounds,
                // it starts the 'FirstGoChooser' func and 

                Debug.Log("Attempting to find next player from player number " + whoseGo);

                
                bool stopCheck = false;
                for (int ticker = whoseGo + 1; stopCheck != true; ticker++)
                {
                    Debug.Log("Checking player " + ticker);
                    if(ticker >= playerAmount) // if the end of the amount of fellers in the array is reached, shut everything down and go into the 'firstgo' func.
                    {
                        stopCheck = true;
                        currentTurn++;
                        FirstGoChooser();
                        goEnded = false;
                        yield return null;
                    }
                    else
                    {
                        GameObject obj = playerEntities[ticker];
                        if (obj.GetComponentInChildren<PlayerStats>().isAlive == true)
                        {
                            stopCheck = true;
                            whoseGo = ticker;
                            Go();
                        }
                    }
                }
                goEnded = false;
                #endregion

            }

            yield return new WaitForEndOfFrame();
        }
    }


    /// <summary>
    /// This function looks through all available fellers to determine who should go first.
    /// </summary>
    private void FirstGoChooser()
    {
        Debug.Log("Choosing the first player.");

        bool stopCheck = false;
        for(int ticker = 0; stopCheck != true; ticker++)
        {
            Debug.Log("Checking player " + ticker);

            GameObject obj = playerEntities[ticker];
            if (obj.GetComponentInChildren<PlayerStats>().isAlive == true)
            {
                Debug.Log("Player number " + ticker + " is a suitable candidate for first go.");
                stopCheck = true; // kills the loop
                whoseGo = ticker;
            }
            else
            {
                Debug.Log("Player number " + ticker + "is not a suitable candidate for first go.");
            }
        }

        Go();
    }

    public void Go()
    {
        Debug.Log("It is now player " + whoseGo + "'s turn.");
        ChangeTurnText(currentTurn, whoseGo);

        ToggleFunc(whoseGo, true);

        PlayerStats currentStats = playerEntities[whoseGo].GetComponentInChildren<PlayerStats>();
        ChangeStatText(currentStats.moveEnergy, currentStats.atckEnergy, currentStats.currentHealth, currentStats.maxHealth);

        gridComp.SetUpGo(currentStats.gridPos);
    }
    #endregion

    // LUX
}