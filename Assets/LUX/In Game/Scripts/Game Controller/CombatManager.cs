using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region Variables

    public bool debug;

    public int damMin; // changed by turn system
    public int damMax; // changed by turn system
    public Vector2 playerPos = Vector2.zero; // changed by turn system
    public int playerNum = -69; // changed by turn system

    private LUX_Grid gridComp;

    #endregion


    #region Functions

    private void Start()
    {
        gridComp = gameObject.GetComponent<LUX_Grid>();
    }

    private void ApplyDamage(string direction)
    {
        if(gameObject.GetComponent<GameController>().alivePlayers[playerNum].GetComponentInChildren<PlayerStats>().atckEnergy != 0)
        {
            #region Deduct Energy

            gameObject.GetComponent<GameController>().alivePlayers[playerNum].GetComponentInChildren<PlayerStats>().atckEnergy--;
            GameObject.FindGameObjectWithTag("AttackText").GetComponent<TextMeshProUGUI>().SetText("ATK - " + gameObject.GetComponent<GameController>().alivePlayers[playerNum].GetComponentInChildren<PlayerStats>().atckEnergy.ToString());

            #endregion

            #region Generate Damage Amount

            float tempDam = Random.Range(damMin, damMax);
            int trueDam = Mathf.RoundToInt(tempDam);
            if (debug) { Debug.Log("Used " + damMin + " and " + damMax + " to generate " + tempDam + " (rounded to " + trueDam + ")"); }

            #endregion

            #region Attempt to Apply Damage

            int xChange = 0;
            int yChange = 0;

            if (direction == "up")
            {
                yChange = 1;
            }
            else if (direction == "right")
            {
                xChange = 1;
            }
            else if (direction == "down")
            {
                yChange = -1;
            }
            else if (direction == "left")
            {
                xChange = -1;
            }

            Vector2 playerAttacking = new Vector2(playerPos.x + xChange, playerPos.y + yChange);

            if (playerAttacking.x >= gridComp.gridWidth || playerAttacking.y >= gridComp.gridHeight || playerAttacking.x == -1 || playerAttacking.y == -1) // if out of bounds
            {
                OutcomeMiss("because that spot was out of bounds.");
            }
            else
            {
                LUX_GridBit specificGridBit = gridComp.localGridArray[(int)playerAttacking.x, (int)playerAttacking.y].GetComponent<LUX_GridBit>();

                if (specificGridBit.playerOnThis == true)
                {
                    int who = specificGridBit.playerNumOnThis;
                    GameObject whoGameObj = gameObject.GetComponent<GameController>().alivePlayers[who];
                    PlayerStats whoStats = whoGameObj.GetComponentInChildren<PlayerStats>();
                    whoStats.currentHealth = whoStats.currentHealth - trueDam;

                    if (debug)
                    {
                        Debug.Log("Player number " + playerNum + " has attacked in the direction of " + direction + ".");
                        Debug.Log("This attack has hit " + whoStats.playerNum + " for " + trueDam + ".");
                    }

                    bool didKill = false;

                    #region HIT
                    int whoHealth = whoStats.currentHealth;
                    int tempHealthVar = whoHealth;
                    whoHealth = whoHealth - trueDam;


                    if (debug) { Debug.Log("Player number " + who + " has taken " + trueDam + " damage."); }
                    #endregion

                    #region KILL
                    if (whoStats.currentHealth <= 0)
                    {
                        if (debug) { Debug.Log("Player " + playerNum + " has killed and will kill again."); }

                        didKill = true;
                        whoStats.isAlive = false;

                        whoGameObj.GetComponent<MeshRenderer>().enabled = false; //rip bozo
                        whoGameObj.GetComponentInChildren<Text_HandlingFeller>().playerName.gameObject.SetActive(false);
                        specificGridBit.playerOnThis = false;
                        specificGridBit.playerNumOnThis = -69;

                        gameObject.GetComponent<LUX_Grid>().SetAllVisitedNegative(playerPos);
                        gameObject.GetComponent<LUX_Grid>().SetDistance(playerPos);

                        gameObject.GetComponent<GameFinisher>().deadFellers++;
                        gameObject.GetComponent<GameFinisher>().CheckForEndQualifications();
                    }
                    #endregion

                    #region ACCREDATION

                    PlayerStats attacker = gameObject.GetComponent<GameController>().alivePlayers[playerNum].GetComponentInChildren<PlayerStats>();

                    if (didKill)
                    {
                        // only give the players the old current health. Damage taken in excess of health will not count.

                        attacker.totalDamage = attacker.totalDamage + tempHealthVar;
                        whoStats.totalHealthLost = whoStats.totalHealthLost + tempHealthVar;
                        StartCoroutine(whoGameObj.GetComponentInChildren<Text_HandlingFeller>().TookDam(trueDam.ToString() + " KILLED!"));
                    }
                    else
                    {
                        attacker.totalDamage = attacker.totalDamage + trueDam;
                        whoStats.totalHealthLost = whoStats.totalHealthLost + trueDam;
                        StartCoroutine(whoGameObj.GetComponentInChildren<Text_HandlingFeller>().TookDam(trueDam.ToString()));
                        StartCoroutine(whoGameObj.GetComponent<WibbleWobblah>().OnHit());
                    }

                    #endregion
                }
                else
                {
                    OutcomeMiss("because nobody was on that spot.");
                }
            }
            #endregion
        }
        else
        {
            OutcomeMiss("because you had no energy.");
        }
    }

    private void OutcomeMiss(string reason)
    {
        Debug.Log("The player missed " + reason);
    }

    #endregion

    #region Input Output

    public void ReceiveSelectionAsUp()
    {
        if(debug) { DebugDirectional("up"); }
        ApplyDamage("up");
    }

    public void ReceiveSelectionAsRight()
    {
        if (debug) { DebugDirectional("right"); }
        ApplyDamage("right");
    }

    public void ReceiveSelectionAsDown()
    {
        if (debug) { DebugDirectional("down"); }
        ApplyDamage("down");
    }

    public void ReceiveSelectionAsLeft()
    {
        if (debug) { DebugDirectional("left"); }
        ApplyDamage("left");
    }

    private void DebugDirectional(string dir)
    {
        Debug.Log("Player has chosen to attack in the direction of " + dir + ".");
    }

    #endregion
}