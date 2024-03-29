using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// This script is used for the GameLoop and Lobby scenes.
/// </summary>
public class SAVEMaster : MonoBehaviour
{
    SAVEHardData saveData = new SAVEHardData();
    public bool saveWasFound = false;

    private void Awake()
    {
        //if (MetaStats.isLoadedFromSave == false)
        //{
            //saveData = new SAVEHardData();
       //}
    }

    public void Load()
    {
        saveData = SAVEAndLoader.instance.LoadGame();

        if(saveData == null)
        {
            saveWasFound = false;
        }
        else
        {
            saveWasFound = true;

            MetaStats.isLoadedFromSave = true;
            MetaStats.gridbitGen = saveData.gridBit;
            MetaStats.playerNames = saveData.playerName;
            MetaStats.playerHealth = saveData.playerHealth;
            MetaStats.playerDamageDealt = saveData.playerDamDealt;
            MetaStats.playerHealthLost = saveData.playerDamRecieved;
            MetaStats.playerGridPosX = saveData.playerPosX;
            MetaStats.playerGridPosY = saveData.playerPosY;
            MetaStats.turnGoAmount = saveData.goAmount;
            MetaStats.turnAmount = saveData.turnAmount;
        }
    }

    public void Save()
    {
        //SAVEHardData tempSaveData = new SAVEHardData();

        saveData.gridBit = MetaStats.gridbitGen;
        saveData.playerName = MetaStats.playerNames;
        saveData.playerHealth = MetaStats.playerHealth;
        Debug.Log("Player ratios are being loaded saved as: 0 - " + MetaStats.playerDamageDealt[0] + "/" + MetaStats.playerHealthLost[0] + " 1 - " + MetaStats.playerDamageDealt[1] + "/" + MetaStats.playerHealthLost[1] + " 2 - " + MetaStats.playerDamageDealt[2] + "/" + MetaStats.playerHealthLost[2]);
        saveData.playerDamDealt = MetaStats.playerDamageDealt;
        saveData.playerDamRecieved = MetaStats.playerHealthLost;
        saveData.playerPosX = MetaStats.playerGridPosX;
        saveData.playerPosY = MetaStats.playerGridPosY;
        saveData.goAmount = MetaStats.turnGoAmount;
        saveData.turnAmount = MetaStats.turnAmount;

        SAVEAndLoader.instance.SaveGame(saveData);
    }

    public void ChangePlayerName(int playerNum, string chosenName)
    {
        Debug.Log("SAVEMaster : Player number " + playerNum + " has chosen " + chosenName);
        MetaStats.playerNames[playerNum] = chosenName;
        Debug.Log("SAVEAndLoader : Player number " + playerNum + " has saved their name to be " + chosenName);
        Save();
    }

    public void CreateNew()
    {
        SAVEHardData newSave = new SAVEHardData(); // creates new
        SAVEAndLoader.instance.SaveGame(newSave); // overwrites whatever is inside with new
        saveData = newSave;
    }
}