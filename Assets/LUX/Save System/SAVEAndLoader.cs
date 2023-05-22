using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SAVEAndLoader : MonoBehaviour
{
    #region Singleton Statement
    static public SAVEAndLoader instance;
    #endregion

    SAVEHardData saveData;
    string filePath;

    private void Awake()
    {
        #region SingleTon Logic
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        filePath = Application.persistentDataPath + "/save.Ldata";
    }

    public void SaveGame(SAVEHardData saveData)
    {
        FileStream dataStream = new FileStream(filePath, FileMode.Create); // this creates the file anew in the path and opens the filestream

        BinaryFormatter converter = new BinaryFormatter();

        converter.Serialize(dataStream, saveData); // This line actually takes the data, turns it into binary and saves it into the computer.
        dataStream.Close(); // this closes the filestream ALWAYS CLOSE THE FILESTREAM
    }

    public SAVEHardData LoadGame()
    {
        if (File.Exists(filePath))
        {
            FileStream dataStream = new FileStream(filePath, FileMode.Open); // this accesses the file in the filePath

            BinaryFormatter converter = new BinaryFormatter();
            SAVEHardData saveData = converter.Deserialize(dataStream) as SAVEHardData; // this turns the binary back into the TestGameData class. This isn't instantly recognised and has to be declared 'as';

            dataStream.Close();
            return saveData;
        }
        else
        {
            Debug.LogWarning("SaveFile not found in " + filePath);
            return null;
        }
    }

    public void SetName(int playerNum, string chosenName)
    {
        SAVEHardData data = LoadGame();
        data.playerName[playerNum] = chosenName;
        SaveGame(data);
        Debug.Log("SAVEAndLoader : Player number " + playerNum + " has saved their name to be " + chosenName);
    }
}
