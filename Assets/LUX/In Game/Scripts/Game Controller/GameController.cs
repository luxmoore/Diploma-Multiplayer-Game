using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>This script is like the referee, taking in all of the rules of the game and ensuring that all play abides by them. Only holds functions to activate other functions in separate scripts.</summary>

public class GameController : MonoBehaviour
{
    public int[,] startPlayerList = new int[3,1]; // takes two ints, the first number is the player number and the second number is the player's class. Current values are temporary
    public List<GameObject> alivePlayers;
    private LUX_Grid gridComp;
    [SerializeField] GameObject playerPrefab;
    private UI_BigMan ui_bigBoy;
    public bool[] tempBullShitGridGen = new bool[(MetaStats.gridGenMaxX * MetaStats.gridGenMaxY)]; // TEMP


    private void Awake()
    {
        for(int ticker = 0; ticker < startPlayerList.GetLength(0); ticker ++) // the getlength() function works specifically for multidimensional arrays
        {
            // first create the correct amount of player prefabs, then assign variables to them.
            GameObject playerPre = Instantiate(playerPrefab);
            PlayerStats playerPreContr = playerPre.GetComponentInChildren<PlayerStats>();
            playerPreContr.playerNum = ticker;
            playerPreContr.givenName = MetaStats.playerNames[ticker]; // used regardless of loading or not, as it is the only way the data is saved between scenes

            if (MetaStats.isLoadedFromSave)
            {
                // load
                playerPreContr.gridPos = new Vector2(MetaStats.playerGridPosX[ticker], MetaStats.playerGridPosY[ticker]);

            }
            else
            {
                // create new
                playerPreContr.isAlive = true;

            }

            Text_HandlingFeller textStuff = playerPreContr.GetComponentInChildren<Text_HandlingFeller>();
            textStuff.playerName.SetText(playerPreContr.givenName);
            textStuff.playerName.ForceMeshUpdate();

            alivePlayers.Add(playerPre);
        }
    }

    private void Start()
    {
        gridComp = gameObject.GetComponent<LUX_Grid>();
        ui_bigBoy = gameObject.GetComponent<UI_BigMan>();
        GameSetUp();
    }

    private void GameSetUp()
    {
        // At the start of the game:
        // Generate a grid (the function also places players on that grid)
        // Begin turn one, go one - turning on the appropiate player controller (this is done through the turnsystem script)

        PlayerStats playerRep = alivePlayers[MetaStats.turnGoAmount].GetComponentInChildren<PlayerStats>();
        if(MetaStats.isLoadedFromSave)
        {
            Debug.Log("Attempting to load a grid");

            #region TEMP - FAKING WHOLLY WALKABLE GRID
            for (int i = 0; i < tempBullShitGridGen.Length; i++)
            {
                tempBullShitGridGen[i] = true;
                Debug.Log("Set bit " + i + " to be true");
            }
            #endregion

            gridComp.GridReInit(tempBullShitGridGen, MetaStats.gridGenMaxX);
        }
        else
        {
            // create new
            gridComp.GridGen(MetaStats.gridGenMaxX, MetaStats.gridGenMaxY, MetaStats.gridHolesAmount);
        }
        gridComp.SetAllVisitedNegative(playerRep.gridPos); // error here

        ui_bigBoy.ChangeOver(playerRep.moveEnergy, playerRep.atckEnergy, playerRep.currentHealth, playerRep.maxHealth);
    }

    // LUX
}