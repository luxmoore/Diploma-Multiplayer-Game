using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMaster : MonoBehaviour
{
    TestGameData saveData = new TestGameData();

    void PrintScore()
    {
        Debug.Log("Score: " + saveData.score);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            saveData.AddScore(-1);
            PrintScore();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            saveData.AddScore(1);
            PrintScore();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveTestScript.instance.SaveGame(saveData);
            PrintScore();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            saveData = SaveTestScript.instance.LoadGame();
            PrintScore();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            saveData.ResetData();
        }
    }
}