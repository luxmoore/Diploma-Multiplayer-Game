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
    SAVEHardData saveData;
    public bool saveWasFound = false;

    private void Awake()
    {
        if (MetaStats.isLoadedFromSave == false)
        {
            saveData = new SAVEHardData();
        }
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
            MetaStats.isLoadedFromSave = true;
            MetaStats.gridbitGen = saveData.gridBit;
            MetaStats.playerNames = saveData.playerName;
            MetaStats.playerHealth = saveData.playerHealth;
            MetaStats.playerDamageDealt = saveData.playerDamDealt;
            MetaStats.playerHealthLost = saveData.playerDamRecieved;
            MetaStats.turnGoAmount = saveData.goAmount;
            MetaStats.turnAmount = saveData.turnAmount;
        }
    }

    public void Save()
    {
        SAVEHardData tempSaveData = new SAVEHardData();

        tempSaveData.gridBit = MetaStats.gridbitGen;
        tempSaveData.playerName = MetaStats.playerNames;
        tempSaveData.playerDamDealt = MetaStats.playerDamageDealt;
        tempSaveData.playerDamRecieved = MetaStats.playerHealthLost;
        tempSaveData.goAmount = MetaStats.turnGoAmount;
        tempSaveData.turnAmount = MetaStats.turnAmount;

        SAVEAndLoader.instance.SaveGame(tempSaveData);
    }

    public void ChangePlayerName(int playerNum, string chosenName)
    {
        Debug.Log("SAVEMaster : Player number " + playerNum + " has chosen " + chosenName);
        SAVEAndLoader.instance.SetName(playerNum, chosenName);
    }

    public void CreateNew()
    {
        SAVEHardData newSave = new SAVEHardData(); // creates new
        SAVEAndLoader.instance.SaveGame(newSave); // overwrites whatever is inside with new

    }
}