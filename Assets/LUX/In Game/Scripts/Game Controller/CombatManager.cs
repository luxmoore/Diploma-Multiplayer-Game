using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region Functions

    public bool debug;

    public int damMin; // changed by turn system
    public int damMax; // changed by turn system
    public Vector2 playerPos = Vector2.zero; // changed by turn system

    private LUX_Grid gridComp;

    #endregion


    #region Functions

    private void Start()
    {
        gridComp = gameObject.GetComponent<LUX_Grid>();
    }

    private void ApplyDamage(string direction)
    {
        #region Generate Damage Amount

        float tempDam = Random.Range(damMin, damMax);
        int trueDam = Mathf.RoundToInt(tempDam);
        if (debug) { Debug.Log("Used " + damMin + " and " + damMax + " to generate " + tempDam + " (rounded to " + trueDam + ")"); }

        #endregion

        #region Apply Damage

        int xChange = 0;
        int yChange = 0;

        if(direction == "up")
        {
            yChange = 1;
        }
        else if(direction == "right")
        {
            xChange = 1;
        }
        else if(direction == "down")
        {
            yChange = -1;
        }
        else if(direction == "left")
        {
            xChange = -1;
        }

        Vector2 playerAttacking = new Vector2(playerPos.x + xChange, playerPos.y + yChange);

        if(playerAttacking.x == gridComp.gridWidth || playerAttacking.y == gridComp.gridHeight) // if out of bounds
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

                if(whoStats.currentHealth <= 0)
                {
                    whoStats.isAlive = false;

                    whoGameObj.GetComponent<MeshRenderer>().enabled = false; //rip bozo
                    specificGridBit.playerOnThis = false;
                    specificGridBit.playerNumOnThis = -1;
                }
            }
            else
            {
                OutcomeMiss("because nobody was on that spot.");
            }
        }
        #endregion
    }

    private void OutcomeMiss(string reason)
    {

    }

    #endregion

    #region Input Output

    public void ReceiveSelectionAsUp()
    {
        string direction = "up";
        ApplyDamage(direction);
    }

    public void ReceiveSelectionAsRight()
    {
        string direction = "right";
        ApplyDamage(direction);
    }

    public void ReceiveSelectionAsDown()
    {
        string direction = "down";
        ApplyDamage(direction);
    }

    public void ReceiveSelectionAsLeft()
    {
        string direction = "left";
        ApplyDamage(direction);
    }

    #endregion
}