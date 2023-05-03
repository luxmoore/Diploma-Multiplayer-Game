using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used for the game loop.
/// </summary>
public class SAVEMaster : MonoBehaviour
{
    SAVEHardData saveData;
    string fileName;
    string filePath;

    private void Awake()
    {
        filePath = "";

        if(MetaStats.isLoadedFromSave == false)
        {
            saveData = new SAVEHardData();
        } 
        else
        {
            saveData = Application.persistentDataPath + "/" + fileName + ".LUX_data"; ; 
        }
    }
}