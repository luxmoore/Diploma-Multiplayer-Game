using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager instance;

    public TestGameData saveData;

    private void Start()
    {
        #region Singleton
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        // attempt to load a save data
        saveData = SaveTestScript.instance.LoadGame();

        // if no save data, create anew
        if(saveData == null)
        {
            saveData = new TestGameData();
        }
    }


}
