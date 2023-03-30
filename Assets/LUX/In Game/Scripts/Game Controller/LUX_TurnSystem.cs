using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LUX_TurnSystem : MonoBehaviour
{
    // --------------------------------------- Variables ---------------------------------------

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

        followingText = GameObject.FindWithTag("FollowingText").GetComponent<FollowingText>();


        for (int ticker = 0; ticker < playerAmountAlive; ticker++)
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

    // --------------------------------------- Coroutines --------------------------------------- 

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

                goEnded = false;
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

                bool stopCheck = false;
                int changeCheck = whoseGo;


                for (int ticker = whoseGo + 1; stopCheck == true; ticker++)
                {
                    GameObject obj = playerEntities[ticker];
                    if(obj.GetComponentInChildren<PlayerStats>().isAlive == true)
                    {
                        stopCheck = true;
                        whoseGo = ticker;
                    }
                }

                if(stopCheck == true && whoseGo == changeCheck) // WARNING CODE FUCKERY DETECTED
                {
                    currentTurn++;
                    FirstGo();
                    yield break;
                }

                #endregion


                Go();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void FirstGo()
    {
        // find the first alive player
        // go from there.

        // say the players at 0 and 1 are not alive, but player 2 is. Starting at 0 or 1 would cause the whole game to break.

        bool stopCheck = false;

        for(int ticker = 0; stopCheck == true; ticker++)
        {
            GameObject obj = playerEntities[ticker];
            if (obj.GetComponentInChildren<PlayerStats>().isAlive == true)
            {
                stopCheck = true; // kills the loop
                whoseGo = ticker;
            }
        }

        Go();
    }

    public void Go()
    {
        ToggleFunc(whoseGo, true);

        PlayerStats currentStats = playerEntities[whoseGo].GetComponentInChildren<PlayerStats>();
        ChangeStatText(currentStats.moveEnergy, currentStats.atckEnergy, currentStats.currentHealth, currentStats.maxHealth);

        gridComp.SetUpGo(currentStats.gridPos);
    }

    // LUX
}