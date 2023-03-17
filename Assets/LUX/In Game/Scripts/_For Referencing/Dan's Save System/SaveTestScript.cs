using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // this is the serialization library path.

public class SaveTestScript : MonoBehaviour
{
    #region SingleTon Vars
    static public SaveTestScript instance;
    #endregion

    #region Save And Load
    private string filePath;
    public string fileName;

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

        filePath = "C//users/documents/saveswhatever"; // a direct file path like this only works on windows.

        filePath = Application.persistentDataPath + "/" + fileName + ".LUX_data"; // this line finds a consistent route folder for the files. This works on all supported export types, including browser.
                                                                                  // the tag at the end declares the name and type of file. I can make my own type of data, readable only by ME!!!
    }

    public void SaveGame(TestGameData saveData)
    {
        FileStream dataStream = new FileStream(filePath, FileMode.Create); // this creates the file anew in the path and opens the filestream

        BinaryFormatter converter = new BinaryFormatter();

        converter.Serialize(dataStream, saveData); // This line actually takes the data, turns it into binary and saves it into the computer.
        dataStream.Close(); // this closes the filestream ALWAYS CLOSE THE FILESTREAM
    }

    public TestGameData LoadGame()
    {
        if(File.Exists(filePath))
        {
            FileStream dataStream = new FileStream(filePath, FileMode.Open); // this accesses the file in the filePath

            BinaryFormatter converter = new BinaryFormatter();
            TestGameData saveData = converter.Deserialize(dataStream) as TestGameData; // this turns the binary back into the TestGameData class. This isn't instantly recognised and has to be declared 'as';

            dataStream.Close();
            return saveData;
        }
        else
        {
            Debug.LogWarning("SaveFile not found in " + filePath);
            return null;
        }
    }
    #endregion
}